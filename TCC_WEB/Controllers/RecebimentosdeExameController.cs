using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TCC_WEB.Data;
using TCC_WEB.Models;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Microsoft.AspNetCore.Authorization;
using MailKit.Security;

namespace TCC_WEB.Controllers
{
    public class RecebimentosdeExameController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public RecebimentosdeExameController(ApplicationDbContext context, IEmailSender emailSender, IHostingEnvironment env)
        {
            _context = context;
            _emailSender = emailSender;
        }


        // GET: RecebimentosdeExame
        [Authorize]
        [Authorize(Roles = "Administrador,Atendente,Médico")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RecebimentosdeExame.Include(r => r.Paciente).Include(r => r.TipodeExame);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpPost]
        [Authorize]
        [Authorize(Roles = "Administrador,Atendente,Médico")]
        public async Task<IActionResult> Index(string txtProcurar)
        {

            if (!String.IsNullOrEmpty(txtProcurar))
                return View(await _context.RecebimentosdeExame.Where(td => (td.Data.ToString("dd/MM/yyyy") + " " + td.TipodeExame.NomedoExame.ToUpper() + " " + td.Paciente.Nome.ToUpper()).Contains(txtProcurar.ToUpper())).
                    Include(r => r.Paciente).Include(r => r.TipodeExame).ToListAsync());

            var applicationDbContext = _context.RecebimentosdeExame.Include(r => r.Paciente).Include(r => r.TipodeExame);
            return View(await applicationDbContext.ToListAsync());

        }

        // GET: RecebimentosdeExame/Details/5
        [Authorize]
        [Authorize(Roles = "Administrador,Atendente,Médico")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recebimentodeExame = await _context.RecebimentosdeExame
                .Include(r => r.Paciente)
                .Include(r => r.TipodeExame)
                .FirstOrDefaultAsync(m => m.RecebimentodeExameId == id);
            if (recebimentodeExame == null)
            {
                return NotFound();
            }

            return View(recebimentodeExame);
        }

        // GET: RecebimentosdeExame/Create
        [Authorize]
        [Authorize(Roles = "Administrador,Médico")]
        public IActionResult Create()
        {
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "Nome");
            ViewData["TipodeExameId"] = new SelectList(_context.TiposdeExame, "TipodeExameId", "NomedoExame");
            return View();
        }

        // POST: RecebimentosdeExame/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Authorize(Roles = "Administrador,Médico")]
        public async Task<IActionResult> Create([Bind("RecebimentodeExameId,Data,TipodeExameId,Dado,PacienteId,recebe")] RecebimentodeExame recebimentodeExame)
        {
            
            if (ModelState.IsValid)
            {
                if (recebimentodeExame.recebe == true)
                {
                    var paciente = (from s in _context.Pacientes
                                    where s.PacienteId == recebimentodeExame.PacienteId
                                    select s.Nome).FirstOrDefault();

                    var exame = (from s in _context.TiposdeExame
                                 where s.TipodeExameId == recebimentodeExame.TipodeExameId
                                 select s.NomedoExame).FirstOrDefault();

                    var email = (from s in _context.Pacientes
                                 where s.PacienteId == recebimentodeExame.PacienteId
                                 select s.Email).FirstOrDefault();

                    string text = paciente.ToString() + " seu " + exame.ToString() + " deu " + recebimentodeExame.Dado.ToString();
                    //string text = recebimentodeExame.Paciente.Nome.ToString() + " seu " + recebimentodeExame.TipodeExame.NomedoExame.ToString() + " deu ";

                    var message = new MimeMessage();
                    //message.From.Add(new MailboxAddress("Thiago", "thiagotcc1234@gmail.com"));
                    message.From.Add(new MailboxAddress("HOSPITAL X", "thiagotcc1234@gmail.com"));
                    //message.To.Add(new MailboxAddress("Thiago Destinatário", "thiagoliveira.engmeca@gmail.com"));
                    message.To.Add(new MailboxAddress(paciente, email.ToString()));
                    message.Subject = "Resultado Exame do Hospital --- ";
                    message.Body = new TextPart("plain")
                    {
                        Text = text
                    };

                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                        client.Connect("smtp.gmail.com", 587);
                        client.Authenticate("thiagotcc1234@gmail.com", "thiagotcc2@");
                        client.Send(message);
                        client.Disconnect(true);
                    }
                }
                    //try
                    //{
                    //    string email = "thiagoliveira.engmeca@gmail.com";
                    //    string assunto = "eita";
                    //    string mensagem = "";

                    //    //email destino, assunto do email, mensagem a enviar
                    //    await _emailSender.SendEmailAsync(email, assunto, mensagem);
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw ex;
                    //}

                    _context.Add(recebimentodeExame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "Nome", recebimentodeExame.PacienteId);
            ViewData["TipodeExameId"] = new SelectList(_context.TiposdeExame, "TipodeExameId", "NomedoExame", recebimentodeExame.TipodeExameId);
            return View(recebimentodeExame);
        }

        // GET: RecebimentosdeExame/Edit/5
        [Authorize]
        [Authorize(Roles = "Administrador,Médico")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recebimentodeExame = await _context.RecebimentosdeExame.FindAsync(id);
            if (recebimentodeExame == null)
            {
                return NotFound();
            }
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "Nome", recebimentodeExame.PacienteId);
            ViewData["TipodeExameId"] = new SelectList(_context.TiposdeExame, "TipodeExameId", "NomedoExame", recebimentodeExame.TipodeExameId);
            return View(recebimentodeExame);
        }

        // POST: RecebimentosdeExame/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Authorize(Roles = "Administrador,Médico")]
        public async Task<IActionResult> Edit(int id, [Bind("RecebimentodeExameId,Data,TipodeExameId,Dado,PacienteId,recebe")] RecebimentodeExame recebimentodeExame)
        {
            if (id != recebimentodeExame.RecebimentodeExameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recebimentodeExame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecebimentodeExameExists(recebimentodeExame.RecebimentodeExameId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "Nome", recebimentodeExame.PacienteId);
            ViewData["TipodeExameId"] = new SelectList(_context.TiposdeExame, "TipodeExameId", "NomedoExame", recebimentodeExame.TipodeExameId);
            return View(recebimentodeExame);
        }

        // GET: RecebimentosdeExame/Delete/5
        [Authorize]
        [Authorize(Roles = "Administrador,Médico")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recebimentodeExame = await _context.RecebimentosdeExame
                .Include(r => r.Paciente)
                .Include(r => r.TipodeExame)
                .FirstOrDefaultAsync(m => m.RecebimentodeExameId == id);
            if (recebimentodeExame == null)
            {
                return NotFound();
            }

            return View(recebimentodeExame);
        }

        // POST: RecebimentosdeExame/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Authorize(Roles = "Administrador,Médico")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recebimentodeExame = await _context.RecebimentosdeExame.FindAsync(id);
            _context.RecebimentosdeExame.Remove(recebimentodeExame);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecebimentodeExameExists(int id)
        {
            return _context.RecebimentosdeExame.Any(e => e.RecebimentodeExameId == id);
        }

        public interface IEmailSender
        {
            Task SendEmailAsync(string email, string subject, string message);
        }

        public class EmailSettings
        {
            public String PrimaryDomain { get; set; }
            public int PrimaryPort { get; set; }
            public String UsernameEmail { get; set; }
            public String UsernamePassword { get; set; }
            public String FromEmail { get; set; }
            public String ToEmail { get; set; }
            public String CcEmail { get; set; }
        }
        public class AuthMessageSender : IEmailSender
        {
            public AuthMessageSender(IOptions<EmailSettings> emailSettings)
            {
                _emailSettings = emailSettings.Value;
            }

            public EmailSettings _emailSettings { get; }

            public Task SendEmailAsync(string email, string subject, string message)
            {
                try
                {
                    Execute(email, subject, message).Wait();
                    return Task.FromResult(0);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            

            public async Task Execute(string email, string subject, string message)
            {
                try
                {
                    string toEmail = string.IsNullOrEmpty(email) ? _emailSettings.ToEmail : email;

                    MailMessage mail = new MailMessage()
                    {
                        From = new MailAddress(_emailSettings.UsernameEmail, "Jose Carlos Macoratti")
                    };

                    mail.To.Add(new MailAddress(toEmail));
                    //mail.CC.Add(new MailAddress(_emailSettings.CcEmail));

                    mail.Subject = "Macoratti .net - " + subject;
                    mail.Body = message;
                    mail.IsBodyHtml = true;
                    mail.Priority = MailPriority.High;

                    //outras opções
                    //mail.Attachments.Add(new Attachment(arquivo));
                    //

                    //using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                    //{
                    //    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    //    smtp.EnableSsl = true;
                    //    await smtp.SendMailAsync(mail);
                    //}
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
