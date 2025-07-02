using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CafeManagementSystem.Business.Operations.Cafe;
using CafeManagementSystem.Business.Operations.Feature;
using CafeManagementSystem.Business.Operations.Feature.Dtos;
using CafeManagementSystem.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CafeManagementSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class FeaturesController : Controller
    {
        private readonly IFeatureService _featureService;

        public FeaturesController(IFeatureService featureService)
        {
            _featureService = featureService;
        }
        [HttpGet("api/Features")]
        public async Task<IActionResult> GetFeatures()
        {
            var features = await _featureService.GetFeatures();
            var featureDtos = features.Select(f => new FeatureDto
            {
                Id = f.Id,
                Title = f.Title
            }).ToList();

            return Ok(featureDtos);
        }
        [HttpPost]
        public async Task<IActionResult> AddFeature(AddFeatureRequest request)
        {
            var addFeatureDto = new AddFeatureDto
            {
                Title = request.Title
            };

            var result = await _featureService.AddFeature(addFeatureDto);

            if (result.IsSucceed)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }
    }
}

