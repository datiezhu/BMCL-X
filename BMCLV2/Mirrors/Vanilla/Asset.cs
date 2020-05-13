﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using BMCLV2.Game;
using BMCLV2.JsonClass;
using BMCLV2.util;

namespace BMCLV2.Mirrors.Vanilla
{
    public class Asset : Interface.Asset
    {
        private const string Server = "http://resources.download.minecraft.net";

        public override async Task<AssetsIndex> GetAssetsIndex(VersionInfo versionInfo, string savePath)
        {
            string assetIndexString;
            var assetIndex = versionInfo.AssetIndex;
            if (assetIndex == null)
                assetIndexString = await Downloader.DownloadStringTaskAsync($"{Server}indexes/{versionInfo.Id}.json");
            else
                assetIndexString = await Downloader.DownloadStringTaskAsync(versionInfo.AssetIndex.Url);
            savePath = Path.Combine(savePath, $"{assetIndex?.Id ?? versionInfo.Assets}.json");
            FileHelper.WriteFile(savePath, assetIndexString);
            // assets的json比较奇葩，不能直接通过反序列化得到
            var jsSerializer = new JavaScriptSerializer();
            var assetsObject = jsSerializer.Deserialize<Dictionary<string, Dictionary<string, AssetsIndex.Assets>>>(assetIndexString);
            return new AssetsIndex
            {
                Objects = assetsObject["objects"]
            };
        }

        public override async Task GetAssetsObject(AssetsIndex.Assets assets, string savePath)
        {
            var uri = $"{Server}/{assets.Path}";
            savePath = Path.Combine(savePath, assets.Path);
            await Downloader.DownloadFileTaskAsync(uri, savePath);
        }
    }
}