namespace Nebulae
{
    /// <summary>
    /// 热更资源使用模式
    /// </summary>
    public enum EHotResMode
    {
        /// <summary>
        /// 从网络资源目录获取资源（最终发布时使用的模式）
        /// [依赖网络]
        /// </summary>            
        NET_ASSET_BUNDLE,
        /// <summary>
        /// 从本地资源目录获取资源（测试AB资源加载时使用的模式）
        /// [不依赖网络]
        /// </summary>            
        LOCAL_ASSET_BUNDLE,
        /// <summary>
        /// 使用AssetDataBase接口加载资源（开发时使用的模式）
        /// [不依赖网络]
        /// </summary>
        ASSET_DATA_BASE,
    }
}