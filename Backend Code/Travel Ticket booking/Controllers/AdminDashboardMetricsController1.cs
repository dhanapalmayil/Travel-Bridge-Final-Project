using Microsoft.AspNetCore.Mvc;
using Travel_Ticket_booking.Model;
using Travel_Ticket_booking.Service;

[Route("api/[controller]")]
[ApiController]
public class AdminDashboardMetricsController1 : ControllerBase
{
    private readonly AdminDashboardMetricsService _service;

    public AdminDashboardMetricsController1(AdminDashboardMetricsService service)
    {
        _service = service;
    }

    // GET: api/AdminDashboardMetrics
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdminDashboardMetrics>>> GetAdminDashboardMetrics()
    {
        var metrics = await _service.GetAllMetricsAsync();
        return Ok(metrics);
    }

    // GET: api/AdminDashboardMetrics/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AdminDashboardMetrics>> GetAdminDashboardMetrics(int id)
    {
        var metric = await _service.GetMetricByIdAsync(id);

        if (metric == null)
        {
            return NotFound();
        }

        return Ok(metric);
    }

    // PUT: api/AdminDashboardMetrics/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAdminDashboardMetrics(int id, AdminDashboardMetrics adminDashboardMetrics)
    {
        try
        {
            await _service.UpdateMetricAsync(id, adminDashboardMetrics);
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
        catch (Exception)
        {
            if (!(await _service.GetMetricByIdAsync(id) is not null))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/AdminDashboardMetrics
    [HttpPost]
    public async Task<ActionResult<AdminDashboardMetrics>> PostAdminDashboardMetrics(AdminDashboardMetrics adminDashboardMetrics)
    {
        await _service.AddMetricAsync(adminDashboardMetrics);
        return CreatedAtAction(nameof(GetAdminDashboardMetrics), new { id = adminDashboardMetrics.MetricID }, adminDashboardMetrics);
    }

    // DELETE: api/AdminDashboardMetrics/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdminDashboardMetrics(int id)
    {
        var metric = await _service.GetMetricByIdAsync(id);
        if (metric == null)
        {
            return NotFound();
        }

        await _service.DeleteMetricAsync(id);
        return NoContent();
    }
}
