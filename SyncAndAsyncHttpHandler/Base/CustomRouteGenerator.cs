using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace SyncAndAsyncHttpHandler.Base
{
    /// <summary>
    /// 生成路由和实现类的映射
    /// </summary>
    public static class CustomRouteGenerator
    {
        /// <summary>
        /// 默认路由
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static RouteBase GetRoute<T>() where T : IHttpHandler
        {
            var type = typeof(T);
            return new Route("{*route}", Activator.CreateInstance(typeof(CustomRouteHandler<>).MakeGenericType(type)) as IRouteHandler);
        }

        /// <summary>
        /// 自定义路由：映射URL及实现类
        /// </summary>
        /// <param name="url"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static RouteBase GetRoute<T>(string url) where T : IHttpHandler
        {
            var type = typeof(T);
            return new Route(url, Activator.CreateInstance(typeof(CustomRouteHandler<>).MakeGenericType(type)) as IRouteHandler);
        }
    }

    public class CustomRouteHandler<T> : IRouteHandler where T : IHttpHandler, new()
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new T();
        }
    }
}