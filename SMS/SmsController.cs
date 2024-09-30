using ShopOnline.Models;
using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;

public class SmsController : Controller
{
   
    [HttpGet]
    public ActionResult ReceiveSms()
    {
        // Lấy các tham số 
       var userId = Request.QueryString["User_ID"];
        var serviceId = Request.QueryString["Service_ID"];
        var commandCode = Request.QueryString["Command_Code"];
      var message = Request.QueryString["Message"];
       var requestId = Request.QueryString["Request_ID"];

        // Kiểm tra tính hợp lệ của dữ liệu
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(serviceId) || string.IsNullOrEmpty(commandCode) || string.IsNullOrEmpty(message) || string.IsNullOrEmpty(requestId))
        {
            var errorResponse = new SmsResponse
            {
                Status = 1,  // Mã lỗi
                Message = "Tin nhắn không hợp lệ"
            };
            return Json(errorResponse, JsonRequestBehavior.AllowGet);
        }
        // Thêm kiểm tra  số điện thoại
        if (!userId.StartsWith("84"))
        {
            return Json(new SmsResponse { Status = 1, Message = "Số điện thoại không hợp lệ" }, JsonRequestBehavior.AllowGet);
        }

        // Lưu dữ liệu vào cơ sở dữ liệu
        var smsMessage = new SmsMessage
        {
            User_Id = userId,
            Service_Id = serviceId,
            Command_Code = commandCode,
            Message = message,
            Request_Id = requestId,
            ReceivedDate = DateTime.Now  

        };

        bool saveResult = SaveToDatabase(smsMessage);
        if (!saveResult)
        {
            var errorResponse = new SmsResponse
            {
                Status = 2,  // Mã lỗi
                Message = "Có lỗi khi lưu dữ liệu"
            };
            return Json(errorResponse, JsonRequestBehavior.AllowGet);
        }

     
        var successResponse = new SmsResponse
        {
            Status = 0,  // gửi và lưu thành công
            Message = "Bạn gửi tin nhắn thành công.Chúng tôi sẽ liên hệ với bạn sớm nhất..."
        };
        return Json(successResponse, JsonRequestBehavior.AllowGet);
    }

    // Lưu thông tin SMS vào cơ sở dữ liệu
    private bool SaveToDatabase(SmsMessage smsMessage)
    {
        // Logic lưu vào cơ sở dữ liệu (cần thêm Entity Framework hoặc ADO.NET để kết nối SQL Server)
        using (var db = new ShopOnlineEntities())
        {
            db.SmsMessages.Add(smsMessage);
            db.SaveChanges();
        }

        return true; 
    }

   

}
