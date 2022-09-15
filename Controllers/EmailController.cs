using Courseeee.DAL;
using Courseeee.Helpers;
using Courseeee.Models;
using Microsoft.AspNetCore.Mvc;
using QuickMailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courseeee.Controllers
{
    public class EmailController : Controller
    {
        
        public  IActionResult Send()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(MailMessage mailMessage)
        {
            string mgs = "Email göndərilməsində xəta baş verdi";
           
            try
            {
                
               
               await  Helper.SendEmailMessageAsync(mailMessage.Başliq, mailMessage.Metn,mailMessage.Kime);
               
                    mgs = "Mesaj uğurla göndərildi";
               
               
            }
            catch (Exception e)
            {

                mgs = e.Message;
            }
            ViewBag.Mgs = mgs;
            return View();
        }
    }
}
