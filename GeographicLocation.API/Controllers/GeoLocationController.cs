using GeographicLocation.Core;
using GeographicLocation.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace GeographicLocation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeoLocationController : ControllerBase
    {
        private readonly ILogger<GeoLocationController> _logger;
        private ILocationService _locationService;
     

        public GeoLocationController(ILogger<GeoLocationController> logger, ILocationService locationService)
        {
            _logger = logger;
            _locationService = locationService;
        }

        /// <summary>
        /// Returns the IP's location
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        [HttpGet("{IP}")]
        public async Task<IActionResult> GetGeoLocation(string IP)
        {
            #region Validation
            if (string.IsNullOrWhiteSpace(IP) || !_locationService.IsValidIpAddress(IP))
            {
                return BadRequest($"{HttpClientConstants.InvalidIP}");
            }
            #endregion

            try
            {
                _logger.LogInformation("Retrieving IP Information");

                var response = await _locationService.CreateIPRequest(IP);
                if (response == null)
                {   
                    return NotFound("No data found for the provided IP Address");
                }

                _logger.LogInformation("Successfully retrieved IP Informaiton");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"An error has occurred: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBatchList([FromBody] List<IPRequest> IPs)
        {
            #region Validation
            var validIPs = _locationService.GetValidIpAddresses(IPs);
            if (validIPs.Count == 0)
            {
                return BadRequest($"{HttpClientConstants.InvalidListIP}");
            }
            #endregion

            try
            {
                _logger.LogInformation("Creating a batch job");

                var response = await _locationService.CreateBatchJobAsync(IPs);
                if (response == null)
                {
                    return NotFound("Batch job could not be created");
                }

                _locationService.ProcessJob(response);

                _logger.LogInformation("Successfully created a batch job");

                //return CreateAtRoute
                var url = ($"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host}{this.HttpContext.Request.Path}/BatchJobId/{response.Id}");
                return Ok(url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"An error has occurred: {ex.Message}");
            }
        }

        [HttpGet("BatchJobId/{BatchJobId}")]
        public async Task<IActionResult> GetBatchJobStatus(string BatchJobId)
        {
            #region Validation
            if (string.IsNullOrWhiteSpace(BatchJobId))
            {
                return BadRequest("BatchJobId is required.");
            }
            #endregion

            try
            {
                Guid.TryParse(BatchJobId, out Guid jobId);

                _logger.LogInformation("Getting batch job status");

                var response = await _locationService.GetBatchJobStatusAsync(jobId);
                if (response == null)
                {
                    return NotFound("No batch job status data found");
                }

                _logger.LogInformation("Succesfully got batch job status");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"An error has occurred: {ex.Message}");
            }
        }
    }
}
