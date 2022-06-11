using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCommon;

public static class EmailTool
{
    public static readonly EmailConfig emailConfig = new();

    public static void SendEmailForNewAdmin(string toEmail, string account, string name, string pwd)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(emailConfig.sender, emailConfig.address));
        message.To.Add(new MailboxAddress(name, toEmail));
        message.Subject = "新人通知";
        message.Body = new TextPart(TextFormat.Html) { Text = @$"<html><p><b>欢迎加入易买科技：</b></p ><br>
            <p>您的账号 {account} <b>初始密码</b>是 </span><span style='font - size: 15px; '><b>{pwd}</b></span> ,请妥善保存并及时修改密码，谢谢。</p ></ html >" };

        using (var smtp = new MailKit.Net.Smtp.SmtpClient())
        {
            smtp.Connect(emailConfig.host, emailConfig.port, SecureSocketOptions.StartTls);// 连接服务器
            smtp.Authenticate(emailConfig.account, emailConfig.pwd); // 验证账号
            smtp.Send(message);  //发送邮件
            smtp.Disconnect(true); //断开连接

        }
    }
}
public class EmailConfig
{
    public string host { get; init; }
    public int port { get; init; }
    public string sender { get; init; }
    public string address { get; init; }
    public string account { get; init; }
    public string pwd { get; init; }
}
