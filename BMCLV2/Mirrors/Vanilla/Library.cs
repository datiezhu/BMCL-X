using System.Threading.Tasks;
using BMCLV2.Game;

namespace BMCLV2.Mirrors.Vanilla
{
  public class Library : Interface.Library
  {
    private const string Server = "https://libraries.minecraft.net/";

    public override async Task DownloadLibrary(LibraryInfo library, string savePath)
    {
      if (library.HasLibrary())
      {
        var url = library.GetLibrary()?.Url;
        if (string.IsNullOrEmpty(url)) url = $"{Server}{library.GetLibraryPath().Replace('\\', '/')}";
        await Downloader.DownloadFileTaskAsync(url, savePath);
      }

      if (library.IsNative)
      {
        var url = library.GetNative().Url;
        if (string.IsNullOrEmpty(url)) url = $"{Server}{library.GetNativePath().Replace('\\', '/')}";
        await Downloader.DownloadFileTaskAsync(url, savePath);
      }
    }
  }
}
