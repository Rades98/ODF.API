using System;

namespace MailingService.Dtos
{
	public class RegistrationEmailDto
	{
		public string Email { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;

		public string Subject { get; set; } = string.Empty;

		public Uri ActivationUrl { get; set; } = new("https://odf.api.cz");

		public string Header { get; set; } = string.Empty;

		public string Info { get; set; } = string.Empty;

		public string ConfirmButtonText { get; set; } = string.Empty;

		public string NotWorkMessage { get; set; } = string.Empty;

		public string Regards { get; set; } = string.Empty;

		public string DeleteNoticeIfSpam { get; set; } = string.Empty;
	}
}
