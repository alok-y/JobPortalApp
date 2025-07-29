using JobPortalReal.Models;
using JobPortalReal.Services.Implementations;
using JobPortalReal.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace JobPortalReal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
           
            
        }
        // GET: api/Notification/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsForUser(string userId)
        {
            var notifications = await _notificationService.GetNotificationsForUserAsync(userId);
            return Ok(notifications);
        }

        [HttpPost]
        public async Task<ActionResult> CreateNotification([FromBody] NotificationDto notificationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notification = new Notification
            {
                UserId = notificationDto.UserId,
                Message = notificationDto.Message,
                CreatedDate = DateTime.UtcNow,
                IsRead = false
            };

            await _notificationService.AddNotificationAsync(notification);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            await _notificationService.DeleteNotificationAsync(notification);
            return Ok();
        }

        [HttpPut("markasread/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _notificationService.MarkNotificationAsReadAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }


        //[HttpGet("markasread/Count/{id}")]
        //public async Task<IActionResult> MarkAsReadCount(string id)
        //{
        //    try
        //    {
        //        var result = await _notificationService.GetUnreadNotificationCountAsync(id); // Assuming this returns an int

        //        if (result == 0)
        //        {
        //            return Ok(new { success = true, message = "No unread notifications found." });
        //        }

        //        return Ok(new { success = true, count = result });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (ex) here if needed
        //        return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
        //    }
        //}

       

        //[HttpGet("getUserId")]
        //public async Task<IActionResult> GetUserId()
        //{
        //    var userId = await _userService.GetUserIdAsync(User);
        //    return Ok(new { success = true, id = userId });
        //}
    }
}