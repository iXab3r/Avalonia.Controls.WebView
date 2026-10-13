using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Avalonia.Controls.Macios.Interop.WebKit;

internal unsafe class WKScriptMessageHandler : NSManagedObjectBase
{
    private static readonly IntPtr s_class;

    private static readonly IntPtr s_messageName = Libobjc.sel_getUid("name");
    private static readonly IntPtr s_frameInfo = Libobjc.sel_getUid("frameInfo");
    private static readonly IntPtr s_request = Libobjc.sel_getUid("request");
    private static readonly IntPtr s_url = Libobjc.sel_getUid("URL");
    private static readonly IntPtr s_absoluteString = Libobjc.sel_getUid("absoluteString");
    private static readonly IntPtr s_isMainFrame = Libobjc.sel_getUid("isMainFrame");
    private static readonly IntPtr s_messageBody = Libobjc.sel_getUid("body");

    private static readonly delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, IntPtr, void>
        s_didReceiveScriptMessage = &OnDidReceiveScriptMessage;

    static WKScriptMessageHandler()
    {
        var delegateClass = AllocateClassPair("ManagedWKScriptMessageHandler");

        if (Libobjc.objc_getProtocol("WKScriptMessageHandler") is var protocol and > 0)
        {
            AddProtocol(delegateClass, protocol);   
        }

        var willPresentNotificationSel = Libobjc.sel_getUid("userContentController:didReceiveScriptMessage:");
        AddMethod(delegateClass, willPresentNotificationSel, new IntPtr(s_didReceiveScriptMessage), "v@:@@");

        RegisterManagedMembers(delegateClass);

        Libobjc.objc_registerClassPair(delegateClass);
        s_class = delegateClass;
    }

    public WKScriptMessageHandler() : base(s_class)
    {
        Init();
    }

    public event EventHandler<ScriptMessageEventArgs>? DidReceiveScriptMessage;

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void OnDidReceiveScriptMessage(IntPtr self, IntPtr sel, IntPtr controller, IntPtr messagePtr)
    {
        var managed = ReadManagedSelf<WKScriptMessageHandler>(self);
        var messageName = NSString.GetString(Libobjc.intptr_objc_msgSend(messagePtr, s_messageName));
        var frame = Libobjc.intptr_objc_msgSend(messagePtr, s_frameInfo);
        var request = Libobjc.intptr_objc_msgSend(frame, s_request);
        var url = Libobjc.intptr_objc_msgSend(request, s_url);
        managed?.DidReceiveScriptMessage?.Invoke(managed, new ScriptMessageEventArgs
        {
            Name = messageName,
            IsMainFrame = Libobjc.byte_objc_msgSend(frame, s_isMainFrame) != 0,
            Source = NSString.GetString(Libobjc.intptr_objc_msgSend(url, s_absoluteString)),
            Body = Libobjc.intptr_objc_msgSend(messagePtr, s_messageBody)
        });
    }

    public class ScriptMessageEventArgs : CancelEventArgs
    {
        public string? Name { get; init; }
        public IntPtr Body { get; init; }
        public string? Source { get; init; }
        public bool IsMainFrame { get; init; }
    }
}
