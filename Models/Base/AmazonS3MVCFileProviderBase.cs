

namespace Syncfusion.EJ2.FileManager.Base
{
    public interface AmazonS3MVCFileProviderBase : FileProviderBase
    {
        void RegisterAmazonS3(string bucketName, string awsAccessKeyId, string awsSecretAccessKey, string bucketRegion);
    }

}
