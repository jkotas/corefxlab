using System.Runtime.InteropServices;

namespace System.Net.Libuv
{
    // This is roughly based on LibuvSharp
    static class UVInterop
    {
        static byte[] ConvertToUtf8(string ip)
        {
            var res = new byte[ip.Length + 1];
            for (int i = 0; i < ip.Length; i++)
                res[i] = (byte)ip[i]; // HACK
            return res;
        }

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr uv_default_loop();

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int uv_loop_init(IntPtr handle);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int uv_loop_close(IntPtr ptr);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr uv_loop_size();

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void uv_run(IntPtr loop, uv_run_mode mode);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void uv_stop(IntPtr loop);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int uv_loop_alive(IntPtr loop);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int uv_is_closing(IntPtr handle);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int uv_handle_size(UVHandle.HandleType type);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        private unsafe static extern void uv_close(IntPtr handle, IntPtr cb);

        internal static void uv_close(IntPtr handle, close_callback cb)
        {
            uv_close(handle, Marshal.GetFunctionPointerForDelegate(cb));
        }

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        private extern unsafe static int uv_ip4_addr(byte * ip, int port, out sockaddr_in address);

        internal unsafe static int uv_ip4_addr(string ip, int port, out sockaddr_in address)
        {
            fixed (byte* pIp = ConvertToUtf8(ip))
            {
                return uv_ip4_addr(pIp, port, out address);
            }
        }

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        private extern unsafe static int uv_ip6_addr(byte* ip, int port, out sockaddr_in6 address);

        internal unsafe static int uv_ip6_addr(string ip, int port, out sockaddr_in6 address)
        {
            fixed (byte* pIp = ConvertToUtf8(ip))
            {
                return uv_ip6_addr(pIp, port, out address);
            }
        }

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        private static extern int uv_listen(IntPtr stream, int backlog, IntPtr callback);

        internal static int uv_listen(IntPtr stream, int backlog, handle_callback callback)
        {
            return uv_listen(stream, backlog, Marshal.GetFunctionPointerForDelegate(callback));
        }

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int uv_accept(IntPtr server, IntPtr client);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern sbyte* uv_strerror(int systemErrorCode);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern sbyte* uv_err_name(int systemErrorCode);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int uv_req_size(RequestType type);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern int uv_tcp_bind(IntPtr handle, void * sockaddr, uint flags);

        internal unsafe static int uv_tcp_bind(IntPtr handle, ref sockaddr_in sockaddr, uint flags)
        {
            fixed (void* pSockAddr = &sockaddr)
                return uv_tcp_bind(handle, pSockAddr, flags);
        }

        internal unsafe static int uv_tcp_bind(IntPtr handle, ref sockaddr_in6 sockaddr, uint flags)
        {
            fixed (void * pSockAddr = &sockaddr)
                return uv_tcp_bind(handle, pSockAddr, flags);
        }

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int uv_tcp_init(IntPtr loop, IntPtr handle);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int uv_tcp_nodelay(IntPtr handle, int enable);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int uv_tcp_keepalive(IntPtr handle, int enable, int delay);

        // [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        // internal static extern int uv_read_start(IntPtr stream, alloc_callback_unix alloc_callback, read_callback_unix read_callback);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        private static extern int uv_read_start(IntPtr stream, IntPtr alloc_callback, IntPtr read_callback);

        internal static int uv_read_start(IntPtr stream, alloc_callback_win alloc_callback, read_callback_win read_callback)
        {
            return uv_read_start(stream, Marshal.GetFunctionPointerForDelegate(alloc_callback), Marshal.GetFunctionPointerForDelegate(read_callback));
        }

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe extern static int uv_try_write(IntPtr handle, void* buffersList, int bufferCount);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        private unsafe static extern int uv_shutdown(IntPtr req, IntPtr handle, IntPtr callback);

        internal static int uv_shutdown(IntPtr req, IntPtr handle, handle_callback callback)
        {
            return uv_shutdown(req, handle, Marshal.GetFunctionPointerForDelegate(callback));
        }

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int uv_read_watcher_start(IntPtr stream, Action<IntPtr> read_watcher_callback);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int uv_read_stop(IntPtr stream);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        private unsafe static extern int uv_write(IntPtr req, IntPtr handle, void* bufferList, int bufferCount, IntPtr callback);

        internal unsafe static int uv_write_unix(IntPtr req, IntPtr handle, UVBuffer.Unix* bufferList, int bufferCount, handle_callback callback)
        {
            return uv_write(req, handle, bufferList, bufferCount, Marshal.GetFunctionPointerForDelegate(callback));
        }

        internal unsafe static int uv_write_win(IntPtr req, IntPtr handle, UVBuffer.Windows* bufferList, int bufferCount, handle_callback callback)
        {
            return uv_write(req, handle, bufferList, bufferCount, Marshal.GetFunctionPointerForDelegate(callback));
        }

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int uv_is_active(IntPtr handle);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        static extern UVHandle.HandleType uv_guess_handle(int fd);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        static extern void uv_ref(IntPtr handle);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        static extern void uv_unref(IntPtr handle);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        static extern int uv_has_ref(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void read_callback_unix(IntPtr stream, IntPtr size, ref UVBuffer.Unix buffer);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void read_callback_win(IntPtr stream, IntPtr size, ref UVBuffer.Windows buffer);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void alloc_callback_unix(IntPtr data, uint size, out UVBuffer.Unix buffer);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void alloc_callback_win(IntPtr data, uint size, out UVBuffer.Windows buffer);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void close_callback(IntPtr handle);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void handle_callback(IntPtr req, int status);
    }

    [StructLayout(LayoutKind.Sequential, Size = 16)]
    struct sockaddr_in
    {
        public int a, b, c, d;
    }

    [StructLayout(LayoutKind.Sequential, Size = 28)]
    struct sockaddr_in6
    {
        public int a, b, c, d, e, f, g;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct uv_handle_t
    {
        public IntPtr data;
        public IntPtr loop;
        public UVHandle.HandleType type;
        public IntPtr close_cb;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct uv_stream_t
    {
        public IntPtr write_queue_size;
        public IntPtr alloc_cb;
        public IntPtr read_cb;
        public IntPtr read2_cb;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct uv_req_t
    {
        public IntPtr data;
        public RequestType type;
    }

    enum RequestType : int
    {
        UV_UNKNOWN_REQ = 0,
        UV_REQ,
        UV_CONNECT,
        UV_WRITE,
        UV_SHUTDOWN,
        UV_UDP_SEND,
        UV_FS,
        UV_WORK,
        UV_GETADDRINFO,
        UV_GETNAMEINFO,
        UV_REQ_TYPE_PRIVATE,
        UV_REQ_TYPE_MAX,
    }

    enum uv_run_mode : int
    {
        UV_RUN_DEFAULT = 0,
        UV_RUN_ONCE,
        UV_RUN_NOWAIT
    };
}
