using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Indeximals.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using System.Linq;

namespace Indeximals.Controllers
{
    public class HomeController : Controller
    {
        private readonly CloudBlobClient _cloudBlobClient;
        private readonly CloudBlobContainer _animalsContainer;

        public HomeController(CloudBlobClient cloudBlobClient)
        {
            _cloudBlobClient = cloudBlobClient;
            _animalsContainer = _cloudBlobClient.GetContainerReference("animals");
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(Animal model)
        {
            if(model?.Id != null)
            {
                await _animalsContainer.CreateIfNotExistsAsync();

                var blob = _animalsContainer.GetBlockBlobReference($"{model.Id}.json");
                blob.Properties.ContentType = "application/json";
                await blob.UploadTextAsync(JsonConvert.SerializeObject(model));
            }

            return View(model);
        }

        public async Task<IActionResult> List()
        {
            await _animalsContainer.CreateIfNotExistsAsync();
            var animalBlobs = await _animalsContainer.ListBlobsSegmentedAsync(null);

            var animalJsons = await Task.WhenAll(animalBlobs.Results
                .OfType<CloudBlockBlob>()
                .Select(x => x.DownloadTextAsync()));

            return View(animalJsons
                .Select(x => JsonConvert.DeserializeObject<Animal>(x))
                .ToArray());
        }

        public async Task<IActionResult> Query()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
