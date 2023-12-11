
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CommonController : ControllerBase
{
    private readonly IFeatureRepository _featureRepository;

    public CommonController(IFeatureRepository featureRepository)
    {
        _featureRepository = featureRepository;
    }

    [HttpGet("{partitionKey}/{rowKey}")]
    public async Task<ActionResult> GetFeature(string partitionKey, string rowKey)
    {
        try
        {
            var feature = await _featureRepository.RetrieveFeatureAsync(partitionKey, rowKey);

            if (feature == null)
            {
                return NotFound();
            }

            FeatureEntity entity = new FeatureEntity
            {
                FeatureName = feature.FeatureName,
                Value = feature.Value
            };

            return Ok(entity);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }


    [HttpGet("search")]
    public async Task<IActionResult> SearchFeatures(string entityName)
    {
        try
        {
            var results = await _featureRepository.SearchFeaturesByEntityAsync(entityName);

            if (results == null || results.Count == 0)
            {
                return NotFound($"No features found for entity: {entityName}");
            }
            else
            {
                List<FeatureEntity> entities = results.Select(result => new FeatureEntity
                {
                    FeatureName = result.FeatureName,
                    Value = result.Value
                }).ToList();

                return Ok(entities);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }   
}











