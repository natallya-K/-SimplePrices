using ReadConfig;
using SimplePrices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SimplePrices.Controllers
{
    public class UploadController : ApiController
    {
        [Route("upload/{numero}")]
        [AllowAnonymous]
        [EnableCors("*", "*", "*")]
        public async Task<HttpResponseMessage> PostProductImage(string numero)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    HttpPostedFile postedFile = httpRequest.Files[file];
                    
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png"};
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {

                            GetConfig config = new GetConfig();
                            string targetDirectory = config.appConfig.ImagePath;
                            if (!Directory.Exists(targetDirectory)) Directory.CreateDirectory(targetDirectory);
                            //var filePath = (config.appConfig.ImagePath + numero + extension);
                            var filePath = (config.appConfig.ImagePath + numero + ".jpg");
                            filePath = filePath.Replace(@"/", "\\");
                            postedFile.SaveAs(filePath);

                  
                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                //var res = string.Format(ex);
                dict.Add("error", ex);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }
    }
}
