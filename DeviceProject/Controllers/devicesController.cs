using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeviceProject.Models;

namespace DeviceProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class devicesController : ControllerBase
    {
        private readonly device_managerContext _context;

        public devicesController(device_managerContext context)
        {
            _context = context;
        }

        // GET: api/devices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<device>>> Getdevices()
        {
            return new OkObjectResult(new
            {
                Success = true,
                Message = "Success",
                Data = await _context.devices.ToListAsync()
            });
        }

        // GET: api/devices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<device>> Getdevice(int id)
        {
            var device = await _context.devices.FindAsync(id);

            if (device == null)
            {
                return new OkObjectResult(new
                {
                    Success = true,
                    Message = "Success",
                    Data = NotFound()
                });
            }
            return new OkObjectResult(new
            {
                Success = true,
                Message = "Success",
                Data = device
            });
        }

        // PUT: api/devices/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putdevice(int id, device device)
        {
            if (id != device.device_id)
            {
                return new OkObjectResult(new
                {
                    Success = false,
                    Message = "false",
                    Data = BadRequest()
                });
            }

            _context.Entry(device).State = EntityState.Modified;

            try
            {
                return new OkObjectResult(new
                {
                    Success = true,
                    Message = "Thay đổi dữ liệu thành công",
                    Data = await _context.SaveChangesAsync()
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!deviceExists(id))
                {
                    return new OkObjectResult(new
                    {
                        Success = false,
                        Message = "false",
                        Data = NotFound()
                    });
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/devices
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<device>> Postdevice(device device)
        {
            _context.devices.Add(device);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                Success = true,
                Message = "Nhập dữ liệu thành công",
                Data = CreatedAtAction("Getdevice", new { id = device.device_id }, device)
            });
        }

        // DELETE: api/devices/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<device>> Deletedevice(int id)
        {
            var device = await _context.devices.FindAsync(id);
            if (device == null)
            {
                return new OkObjectResult(new
                {
                    Success = true,
                    Message = "Xóa dữ liệu thành công",
                    Data = NotFound()
                });
            }

            _context.devices.Remove(device);
            await _context.SaveChangesAsync();
            return new OkObjectResult(new
            {
                Success = true,
                Message = "Xóa dữ liệu thành công",
                Data = device
            });
        }
        [Route("getfilter")]
        [HttpPost]
        public async Task<IActionResult> GetByFilter(devicefilter filter)
        {
            if (filter.FilterAll == null)
            {
                return new OkObjectResult(new
                {
                    Success = true,
                    Message = "All list",
                    Data = await _context.devices.ToListAsync()
                });
            }
            else
            {
                var Data = await _context.devices.Where(x =>
                x.device_name.Contains(filter.FilterAll) ||
                x.device_model_sn.Contains(filter.FilterAll) ||
                x.device_content.Contains(filter.FilterAll) ||
                x.device_location.Contains(filter.FilterAll) ||
                x.device_other.Contains(filter.FilterAll) ||
                x.device_status.Contains(filter.FilterAll) ||
                x.device_user_report.Contains(filter.FilterAll)).ToArrayAsync();
                return new OkObjectResult(new
                {
                    Success = true,
                    Message = "Ok",
                    Data = Data
                });
            }
        }

        private bool deviceExists(int id)
        {
            return _context.devices.Any(e => e.device_id == id);
        }
    }
}
