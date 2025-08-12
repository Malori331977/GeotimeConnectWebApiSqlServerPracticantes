using Microsoft.Extensions.Primitives;

namespace com.gsitcr.geotime.Data
{
    public sealed class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
            // TODO Change the value depending of your needs
            context.Response.Headers.Add("referrer-policy", new StringValues("strict-origin-when-cross-origin"));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
            context.Response.Headers.Add("x-content-type-options", new StringValues("nosniff"));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
            context.Response.Headers.Add("x-frame-options", new StringValues("DENY"));

            // https://security.stackexchange.com/questions/166024/does-the-x-permitted-cross-domain-policies-header-have-any-benefit-for-my-websit
            context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", new StringValues("none"));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-XSS-Protection
            context.Response.Headers.Add("x-xss-protection", new StringValues("1; mode=block"));

            context.Response.Headers.Remove("Server");
            context.Response.Headers.Remove("X-Powered-By");
            context.Response.Headers.Remove("X-AspNet-Version");

            //context.Response.Headers.Add("Strict-Transport-Security", new StringValues("max-age=31536000; includeSubDomains; preload"));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Expect-CT
            // You can use https://report-uri.com/ to get notified when a misissued certificate is detected
            // context.Response.Headers.Add("Expect-CT", new StringValues("max-age=0, enforce, report-uri=\"https://example.report-uri.com/r/d/ct/enforce\""));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Feature-Policy
            // https://github.com/w3c/webappsec-feature-policy/blob/master/features.md
            // https://developers.google.com/web/updates/2018/06/feature-policy
            // TODO change the value of each rule and check the documentation to see if new features are available
            //context.Response.Headers.Add("Feature-Policy", new StringValues(
            //    "accelerometer 'none';" +
            //    "ambient-light-sensor 'none';" +
            //    "autoplay 'none';" +
            //    "battery 'none';" +
            //    "camera 'none';" +
            //    "display-capture 'none';" +
            //    "document-domain 'none';" +
            //    "encrypted-media 'none';" +
            //    "execution-while-not-rendered 'none';" +
            //    "execution-while-out-of-viewport 'none';" +
            //    "gyroscope 'none';" +
            //    "magnetometer 'none';" +
            //    "microphone 'none';" +
            //    "midi 'none';" +
            //    "navigation-override 'none';" +
            //    "payment 'none';" +
            //    "picture-in-picture 'none';" +
            //    "publickey-credentials-get 'none';" +
            //    "sync-xhr 'none';" +
            //    "usb 'none';" +
            //    "wake-lock 'none';" +
            //    "xr-spatial-tracking 'none';"
            //    ));

            //// https://developer.mozilla.org/en-US/docs/Web/HTTP/CSP
            //// https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
            //// TODO change the value of each rule and check the documentation to see if new rules are available
            //context.Response.Headers.Add("Content-Security-Policy", new StringValues(
            //    "block-all-mixed-content;" +
            //    "child-src 'self';" +
            //    "connect-src 'self';" +
            //    "default-src 'self';" +
            //    "img-src 'data: https:';" +
            //    "font-src 'self';" +
            //    "form-action 'self';" +
            //    "frame-ancestors 'self';" +
            //    "frame-src 'self';" +
            //    "manifest-src 'self';" +
            //    "media-src 'self';" +
            //    "object-src 'none';" +
            //    "sandbox;" +
            //    "script-src 'self';" +
            //    "script-src-attr 'self';" +
            //    "script-src-elem 'self';" +
            //    "style-src 'self';" +
            //    "style-src-attr 'self';" +

            //    "upgrade-insecure-requests;" +
            //    "worker-src 'self';"
            //    ));

            return _next(context);
        }
    }
}

