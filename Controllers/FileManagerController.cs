using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Syncfusion.EJ2.FileManager.Base;
using Syncfusion.EJ2.FileManager.AmazonS3MVCFileProvider;

namespace EJ2ASPMVCFileProvider.Controllers
{
    public class FileManagerController : Controller
    {
        public ActionResult FileManager()
        {
            return View();
        }

        AmazonS3FileProvider operation = new AmazonS3FileProvider();
        public FileManagerController()
        {
            this.operation.RegisterAmazonS3( "bucketName",  "awsAccessKeyId",  "awsSecretAccessKey", "bucketRegion");
        }

        public ActionResult FileOperations(FileManagerDirectoryContent args)
        {
            switch (args.Action)
            {
                case "read":
                    // Reads the file(s) or folder(s) from the given path.
                    return Json(operation.ToCamelCase(operation.GetFiles(args.Path, args.ShowHiddenItems)));
                case "delete":
                    // Deletes the selected file(s) or folder(s) from the given path.
                    return Json(operation.ToCamelCase(operation.Delete(args.Path, args.Names)));
                case "details":
                    if (args.Names == null)
                    {
                        args.Names = new string[] { };
                    }
                    // Gets the details of the selected file(s) or folder(s).
                    return Json(operation.ToCamelCase(operation.Details(args.Path, args.Names, args.Data)));
                case "create":
                    // Creates a new folder in a given path.
                    return Json(operation.ToCamelCase(operation.Create(args.Path, args.Name)));
                case "search":
                    // Gets the list of file(s) or folder(s) from a given path based on the searched key string.
                    return Json(operation.ToCamelCase(operation.Search(args.Path, args.SearchString, args.ShowHiddenItems, args.CaseSensitive)));
                case "copy":
                    // Copies the selected file(s) or folder(s) from a path and then pastes them into a given target path.
                    return Json(operation.ToCamelCase(operation.Copy(args.Path, args.TargetPath, args.Names, args.RenameFiles, args.TargetData, args.Data)));
                case "move":
                    // Cuts the selected file(s) or folder(s) from a path and then pastes them into a given target path.
                    return Json(operation.ToCamelCase(operation.Move(args.Path, args.TargetPath, args.Names, args.RenameFiles, args.TargetData,args.Data)));
                case "rename":
                    // Renames a file or folder.
                    return Json(operation.ToCamelCase(operation.Rename(args.Path, args.Name, args.NewName,false,args.Data)));
            }
            return null;
        }

        // Uploads the file(s) into a specified path
        public ActionResult Upload(string path, IList<System.Web.HttpPostedFileBase> uploadFiles, string action)
        {
            if (path == null)
            {
                return Content("");
            }
            FileManagerResponse uploadResponse;
            uploadResponse = operation.Upload(path, uploadFiles, action, null);
            if (uploadResponse.Error != null)
            {
                HttpResponse Response = System.Web.HttpContext.Current.Response;
                Response.Clear();
                Response.Status = uploadResponse.Error.Code + " " + uploadResponse.Error.Message;
                Response.StatusCode = Int32.Parse(uploadResponse.Error.Code);
                Response.StatusDescription = uploadResponse.Error.Message;
                Response.End();
            }
            return Content("");
        }

        // Downloads the selected file(s) and folder(s)
        public ActionResult Download(string downloadInput)
        {
            FileManagerDirectoryContent args = JsonConvert.DeserializeObject<FileManagerDirectoryContent>(downloadInput);
            return operation.Download(args.Path, args.Names);
        }

        // Gets the image(s) from the given path
        public ActionResult GetImage(FileManagerDirectoryContent args)
        {
            return operation.GetImage(args.Path, args.Id, false, null, null);
        }
    }
}