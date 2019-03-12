# Magicodes.Mvc.TrustIp
基于ASP.NET Core的筛选器和中间件打造的通用授信IP通用库,支持筛选器和中间件配置,支持全局和局部使用.

## VNext

* 黑名单
* 支持内存缓存以及分布式缓存
* 本地IP是否授信

## 配置

支持配置多个授信IP,如下"TrustIpList"所示:

{
  "TrustIpList": [ "218.76.8.29", "::1" ]
}

**注意:0.0.0.0 表示支持任意Ip**

## 使用

### 筛选器

#### 启用全局筛选器

services.AddMvc(options => options.Filters.Add(typeof(TrustIPFilter))).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

#### 在控制器中使用

        // GET api/values
        [HttpGet]
        //也可以单独配置
        [ServiceFilter(typeof(TrustIpFilter))]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }


### 中间件

启用中间件:
app.UseTrustIP(loggerFactory, Configuration);

## 结果

如当前请求IP未在授信IP列表之内,则返回http状态码403

## 注意事项

### Nginx 代理不能获取IP问题

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
