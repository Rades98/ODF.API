using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Newtonsoft.Json;
using ODF.AppLayer.Repos;
using ODF.Data.Elastic.Repos.Articles;
using ODF.Data.Elastic.Repos.Contacts;
using ODF.Data.Elastic.Repos.Lineups;
using ODF.Data.Elastic.Repos.Translations;
using ODF.Domain.Entities;
using ODF.Domain.Entities.ContactEntities;

namespace ODF.Data.Elastic.Settings
{
	public static class ElasticSearchRegistrations
	{
		/// <summary>
		/// Configure elastic search
		/// </summary>
		/// <param name="services">services</param>
		/// <param name="configuration">configuration</param>
		/// <returns></returns>
		public static IServiceCollection ConfigureElasticsearch(this IServiceCollection services, IConfiguration configuration)
		{
			var elasticConf = configuration.GetSection(nameof(ElasticsearchSettings)).Get<ElasticsearchSettings>();
			_ = elasticConf ?? throw new ArgumentException(nameof(elasticConf));

			var connectionPool = new StaticConnectionPool(elasticConf.Nodes.Select(node => new Uri(node)));

			var settings = new ConnectionSettings(connectionPool)
				.PrettyJson()
				.DefaultIndex(elasticConf.DefaultIndex)
				.DefaultMappingFor<Article>(m => m.IndexName("odf-articles"))
				.DefaultMappingFor<Translation>(m => m.IndexName("translations"))
				.DefaultMappingFor<LineupItem>(m => m.IndexName("lineup-item"))
				.DefaultMappingFor<Contact>(m => m.IndexName("contact"))
				.BasicAuthentication("elastic", elasticConf.Password);

			var client = new ElasticClient(settings);

			client.Indices.Create("odf-articles", i => i.Map<Article>(x => x.AutoMap()));
			client.Indices.Create("translations", i => i.Map<Translation>(x => x.AutoMap()));
			client.Indices.Create("lineup-item", i => i.Map<LineupItem>(x => x.AutoMap()));
			client.Indices.Create("contact", i => i.Map<Contact>(x => x.AutoMap()));
			client.Seed();

			services.AddSingleton<IElasticClient>(client);

			services.AddTransient<IArticleRepo, ArticleRepo>();
			services.Decorate<IArticleRepo, ArticleRepoCache>();

			services.AddTransient<ITranslationRepo, TranslationRepo>();
			services.Decorate<ITranslationRepo, TranslationRepoCache>();

			services.AddTransient<ILineupRepo, LineupRepo>();
			services.Decorate<ILineupRepo, LineupRepoCahce>();

			services.AddTransient<IContactRepo, ContactRepo>();

			return services;
		}

		private static ElasticClient Seed(this ElasticClient client)
		{
			var contact = new Contact()
			{
				Address = new()
				{
					Street = "Dr. Martínka 1146/27",
					City = "Ostrava-Hrabůvka",
					PostalCode = "700 30",
					Country = "Česká Republika"
				},
				BankAccounts = new[] {
					new BankAccount()
					{
						AccountId = "123-9982410287/0100",
						Bank = "KB",
						IBAN = "cz45 0100 0001 2399 8241 0287"
					}
				},
				ContactPersons = new[]
				{
					new ContactPerson()
					{
						Email = "jiri.machac@folklorova.cz",
						Title = "Ing.",
						Name = "Jiří",
						Surname = "Machač",
						Base64Image = "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",
						Roles = new[] { "Předseda spolku FolklorOVA, z.s.", "Ředitel festivalu a dramaturg ODF"},
						Order = 1,
						Id = Guid.NewGuid(),
					},
					new ContactPerson()
					{
						Email = "dramaturg@folklorova.cz",
						Title = "Ing.",
						Name = "Adam",
						Surname = "Machač",
						Base64Image = "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",
						Roles = new[] { "dramaturg ODF"},
						Order = 2,
						Id = Guid.NewGuid(),
					}
				},
				Email = "info@folklorova.cz",
				EventManager = "FolklorOVA, z.s.",
				EventName = "Festival Ostravské dny folkloru"
			};

			var cont = client.Search<Contact>(x => x.Size(1)).Documents.FirstOrDefault();

			if (cont is null)
			{
				client.Index(contact, i => i);
			}

			List<Translation> sysTrans = new()
			{
				new(){ IsSystem = true, LanguageId = 0, Text ="Jazyk", TranslationCode = "menu_lang"},
				new(){ IsSystem = true, LanguageId = 1, Text ="Language", TranslationCode = "menu_lang"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Sprache", TranslationCode = "menu_lang"},

				new(){ IsSystem = true, LanguageId = 0, Text ="O festivalu", TranslationCode = "menu_about"},
				new(){ IsSystem = true, LanguageId = 1, Text ="About the festival", TranslationCode = "menu_about"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Über das Festival", TranslationCode = "menu_about"},

				new(){ IsSystem = true, LanguageId = 0, Text ="FolklorOVA", TranslationCode = "menu_association"},
				new(){ IsSystem = true, LanguageId = 1, Text ="FolklorOVA", TranslationCode = "menu_association"},
				new(){ IsSystem = true, LanguageId = 2, Text ="FolklorOVA", TranslationCode = "menu_association"},

				new(){ IsSystem = true, LanguageId = 0, Text ="Program", TranslationCode = "menu_lineup"},
				new(){ IsSystem = true, LanguageId = 1, Text ="Festival program", TranslationCode = "menu_lineup"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Festivalprogramm", TranslationCode = "menu_lineup"},

				new(){ IsSystem = true, LanguageId = 0, Text ="Vstupenky", TranslationCode = "menu_tickets"},
				new(){ IsSystem = true, LanguageId = 1, Text ="Tickets", TranslationCode = "menu_tickets"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Tickets", TranslationCode = "menu_tickets"},

				new(){ IsSystem = true, LanguageId = 0, Text ="Kontakt", TranslationCode = "menu_contacts"},
				new(){ IsSystem = true, LanguageId = 1, Text ="Contact", TranslationCode = "menu_contacts"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Kontakt", TranslationCode = "menu_contacts"},

				new(){ IsSystem = true, LanguageId = 0, Text ="Přihlásit se", TranslationCode = "login_user"},
				new(){ IsSystem = true, LanguageId = 1, Text ="Log in", TranslationCode = "login_user"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Anmeldung", TranslationCode = "login_user"},

				new(){ IsSystem = true, LanguageId = 0, Text ="Uživatelské jméno", TranslationCode = "login_username"},
				new(){ IsSystem = true, LanguageId = 1, Text ="User name", TranslationCode = "login_username"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Username", TranslationCode = "login_username"},

				new(){ IsSystem = true, LanguageId = 0, Text ="Heslo", TranslationCode = "login_pw"},
				new(){ IsSystem = true, LanguageId = 1, Text ="Password", TranslationCode = "login_pw"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Passwort", TranslationCode = "login_pw"},

				new(){ IsSystem = true, LanguageId = 0, Text ="Heslo pro kontrolu", TranslationCode = "login_pw2"},
				new(){ IsSystem = true, LanguageId = 1, Text ="Password check", TranslationCode = "login_pw2"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Passwortüberprüfung", TranslationCode = "login_pw2"},

				new(){ IsSystem = true, LanguageId = 0, Text ="e-mail", TranslationCode = "login_email"},
				new(){ IsSystem = true, LanguageId = 1, Text ="e-mail", TranslationCode = "login_email"},
				new(){ IsSystem = true, LanguageId = 2, Text ="e-mail", TranslationCode = "login_email"},

				new(){ IsSystem = true, LanguageId = 0, Text ="Jméno", TranslationCode = "login_first_name"},
				new(){ IsSystem = true, LanguageId = 1, Text ="First name", TranslationCode = "login_first_name"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Vorname", TranslationCode = "login_first_name"},

				new(){ IsSystem = true, LanguageId = 0, Text ="Příjmení", TranslationCode = "login_last_name"},
				new(){ IsSystem = true, LanguageId = 1, Text ="Last name", TranslationCode = "login_last_name"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Nachname", TranslationCode = "login_last_name"},

				new(){ IsSystem = true, LanguageId = 0, Text ="Odhlásit se", TranslationCode = "logout_user"},
				new(){ IsSystem = true, LanguageId = 1, Text ="Log out", TranslationCode = "logout_user"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Ausloggen", TranslationCode = "logout_user"},

				new(){ IsSystem = true, LanguageId = 0, Text ="Nemáte registraci? Klikněte zde!", TranslationCode = "register_user"},
				new(){ IsSystem = true, LanguageId = 1, Text ="Don't have a registration yet? Click here!", TranslationCode = "register_user"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Sie haben keine Registrierung? Klicken Sie hier!", TranslationCode = "register_user"},

				new(){ IsSystem = true, LanguageId = 0, Text ="Přihlášení se nezdařilo", TranslationCode = "login_failed_title"},
				new(){ IsSystem = true, LanguageId = 1, Text ="Login failed", TranslationCode = "login_failed_title"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Fehler bei der Anmeldung", TranslationCode = "login_failed_title"},

				new(){ IsSystem = true, LanguageId = 0, Text ="Zkontrolujte, že jste zadali správné údaje k účtu", TranslationCode = "login_failed_msg"},
				new(){ IsSystem = true, LanguageId = 1, Text ="Check that you have entered the correct account information", TranslationCode = "login_failed_msg"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Überprüfen Sie, ob Sie die richtigen Kontoinformationen eingegeben haben", TranslationCode = "login_failed_msg"},

				new(){ IsSystem = true, LanguageId = 0, Text ="Neoprávněná akce", TranslationCode = "unauthorized_title"},
				new(){ IsSystem = true, LanguageId = 1, Text ="Unauthorized action", TranslationCode = "unauthorized_title"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Unerlaubte Aktion", TranslationCode = "unauthorized_title"},

				new(){ IsSystem = true, LanguageId = 0, Text ="Pro tuto akci nemáte dostatečné oprávnění. Pokud se domníváte, že je máte mít, obraťte se na administrátora.", TranslationCode = "unauthorized_msg_logged"},
				new(){ IsSystem = true, LanguageId = 1, Text ="You do not have sufficient permissions to perform this action. If you think you should have them, please contact an administrator", TranslationCode = "unauthorized_msg_logged"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Sie verfügen nicht über ausreichende Berechtigungen, um diese Aktion auszuführen. Wenn Sie der Meinung sind, dass Sie sie haben sollten, wenden Sie sich bitte an den Administrator.", TranslationCode = "unauthorized_msg_logged"},

				new(){ IsSystem = true, LanguageId = 0, Text ="Pro vykonání této akce je třeba se přihlásit.", TranslationCode = "unauthorized_msg_annonymous"},
				new(){ IsSystem = true, LanguageId = 1, Text ="You must be logged in to perform this action.", TranslationCode = "unauthorized_msg_annonymous"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Sie müssen angemeldet sein, um diese Aktion auszuführen.", TranslationCode = "unauthorized_msg_annonymous"},

				new(){ IsSystem = true, LanguageId = 0, Text ="Nepodporovaný jazyk", TranslationCode = "server_error_wrong_language"},
				new(){ IsSystem = true, LanguageId = 1, Text ="Unsupported language", TranslationCode = "server_error_wrong_language"},
				new(){ IsSystem = true, LanguageId = 2, Text ="Nicht unterstützte Sprache", TranslationCode = "server_error_wrong_language"},

				new(){ IsSystem = true, LanguageId = 0, Text ="Oops, něco se nepovedlo", TranslationCode = "internal_server_error" },
				new(){ IsSystem = true, LanguageId = 1, Text ="Oops, something went wrong", TranslationCode = "internal_server_error" },
				new(){ IsSystem = true, LanguageId = 2, Text ="Hoppla! Etwas ist schiefgelaufen", TranslationCode = "internal_server_error" },

				new(){ IsSystem = true, LanguageId = 0, Text ="Metropole Moravskoslezského kraje a její přilehlé okolí se mohou v listopadu těšit na 1. ročník festivalu Ostravské folklorní dny, které organizuje spolek FolklorOva. Akce, která se bude v centru Ostravy a městských částech konat od středy 8. do neděle 12. listopadu, má obyvatelům představit tradiční lidovou kulturu.", TranslationCode = "about_info" },
				new(){ IsSystem = true, LanguageId = 1, Text ="OIn November, the metropolis of the Moravian-Silesian Region and its surrounding area can look forward to the 1st edition of the Ostrava Folklore Days festival, organized by the FolklorOva association. The event, which will take place in the center of Ostrava and the city districts from Wednesday 8 to Sunday 12 November, is intended to introduce the residents to traditional folk culture.", TranslationCode = "about_info" },
				new(){ IsSystem = true, LanguageId = 2, Text ="Im November kann sich die Metropole der Mährisch-Schlesischen Region und ihre Umgebung auf die 1. Ausgabe des Festivals „Ostrauer Folkloretage“ freuen, das vom Verein FolklorOva organisiert wird. Die Veranstaltung, die vom Mittwoch, 8., bis Sonntag, 12. November, im Zentrum von Ostrava und den Stadtteilen stattfindet, soll den Bewohnern die traditionelle Volkskultur näher bringen.", TranslationCode = "about_info" },

				new(){ IsSystem = true, LanguageId = 0, Text ="Ostravo, těš se na Ostravské dny folkloru!", TranslationCode = "about_header" },
				new(){ IsSystem = true, LanguageId = 1, Text ="Ostrava, look forward to the Ostrava Days of Folklore!", TranslationCode = "about_header" },
				new(){ IsSystem = true, LanguageId = 2, Text ="Ostrava, freuen Sie sich auf die Ostravaer Tage der Folklore!", TranslationCode = "about_header" },

				new(){ IsSystem = true, LanguageId = 0, Text ="Jsme FolklorOVA, spolek nadšenců, kteří chtějí podporovat a dále rozvíjet kulturu v Ostravě a jejím okolí. Skrz akci Ostravské dny folkloru chceme Ostravanům ukázat tradiční lidovou kulturu a věříme, že je nadchne stejně jako nás. Lidová kultura a folklor nezná hranic je tu pro všechny, malé i velké, stejně jako pro staré i mladé.\nFolklor spojuje!", TranslationCode = "association_info" },
				new(){ IsSystem = true, LanguageId = 1, Text ="We are FolklorOVA, an association of enthusiasts who want to support and further develop culture in Ostrava and its surroundings. Through the Ostrava Folklore Days event, we want to show the people of Ostrava traditional folk culture and we believe that they will be as excited as we are. Folk culture and folklore knows no borders and is here for everyone, young and old, as well as old and young.\nFolklore unites!", TranslationCode = "association_info" },
				new(){ IsSystem = true, LanguageId = 2, Text ="Wir sind FolklorOVA, ein Verein von Enthusiasten, die die Kultur in Ostrava und Umgebung unterstützen und weiterentwickeln wollen. Mit der Veranstaltung „Ostrauer Folkloretage“ möchten wir den Menschen in Ostrava die traditionelle Volkskultur näherbringen und glauben, dass sie genauso begeistert sein werden wie wir. Volkskultur und Folklore kennen keine Grenzen und sind für alle da, Jung und Alt, aber auch Alt und Jung.\nFolklore verbindet!", TranslationCode = "association_info" },

				new(){ IsSystem = true, LanguageId = 0, Text ="O nás", TranslationCode = "association_header" },
				new(){ IsSystem = true, LanguageId = 1, Text ="About", TranslationCode = "association_header" },
				new(){ IsSystem = true, LanguageId = 2, Text ="Über uns", TranslationCode = "association_header" },

				new(){ IsSystem = true, LanguageId = 0, Text ="Přepnout do {0}", TranslationCode = "app_language_switch" },
				new(){ IsSystem = true, LanguageId = 1, Text ="Switch to {0}", TranslationCode = "app_language_switch" },
				new(){ IsSystem = true, LanguageId = 2, Text ="Sprache in {0} ändern", TranslationCode = "app_language_switch" },

				new(){ IsSystem = true, LanguageId = 0, Text ="Jazyk", TranslationCode = "app_language" },
				new(){ IsSystem = true, LanguageId = 1, Text ="Language", TranslationCode = "app_language" },
				new(){ IsSystem = true, LanguageId = 2, Text ="Sprache", TranslationCode = "app_language" },

				new(){ IsSystem = true, LanguageId = 0, Text ="Článek, který se pokoušíte zobrazit, byl nejspíše smazán.", TranslationCode = "app_article_notfound" },
				new(){ IsSystem = true, LanguageId = 1, Text ="The article you are trying to view has probably been deleted.", TranslationCode = "app_article_notfound" },
				new(){ IsSystem = true, LanguageId = 2, Text ="Der Artikel, den Sie anzeigen möchten, wurde wahrscheinlich gelöscht.", TranslationCode = "app_article_notfound" },

				new(){ IsSystem = true, LanguageId = 0, Text ="Zdroj nenalezen.", TranslationCode = "app_base_notfound" },
				new(){ IsSystem = true, LanguageId = 1, Text ="Resource not found.", TranslationCode = "app_base_notfound" },
				new(){ IsSystem = true, LanguageId = 2, Text ="Ressource nicht gefunden.", TranslationCode = "app_base_notfound" },

				new(){ IsSystem = true, LanguageId = 0, Text ="Uživatel nenalezen", TranslationCode = "error_login_wrong_user" },
				new(){ IsSystem = true, LanguageId = 1, Text ="User not found", TranslationCode = "error_login_wrong_user" },
				new(){ IsSystem = true, LanguageId = 2, Text ="Benutzer nicht gefunden", TranslationCode = "error_login_wrong_user" },

				new(){ IsSystem = true, LanguageId = 0, Text ="Špatné heslo", TranslationCode = "error_login_wrong_pw" },
				new(){ IsSystem = true, LanguageId = 1, Text ="Wrong password", TranslationCode = "error_login_wrong_pw" },
				new(){ IsSystem = true, LanguageId = 2, Text ="Falsches Passwort", TranslationCode = "error_login_wrong_pw" },

				new(){ IsSystem = true, LanguageId = 0, Text ="Festival Ostravské dny folkloru", TranslationCode = "contact_acc_id" },
				new(){ IsSystem = true, LanguageId = 1, Text ="Festival Ostravské dny folkloru", TranslationCode = "contact_acc_id" },
				new(){ IsSystem = true, LanguageId = 2, Text ="Festival Ostravské dny folkloru", TranslationCode = "contact_acc_id" },

				new(){ IsSystem = true, LanguageId = 0, Text ="Bankovní spojení", TranslationCode = "contact_bank" },
				new(){ IsSystem = true, LanguageId = 1, Text ="Bank account", TranslationCode = "contact_bank" },
				new(){ IsSystem = true, LanguageId = 2, Text ="Bankverbindung", TranslationCode = "contact_bank" },

				new() { IsSystem = true, LanguageId = 0, Text = "IBAN", TranslationCode = "contact_iban" },
				new() { IsSystem = true, LanguageId = 1, Text = "IBAN", TranslationCode = "contact_iban" },
				new() { IsSystem = true, LanguageId = 2, Text = "IBAN", TranslationCode = "contact_iban" },

				new() { IsSystem = true, LanguageId = 0, Text = "Kontakt", TranslationCode = "contact_email" },
				new() { IsSystem = true, LanguageId = 1, Text = "Contact", TranslationCode = "contact_email" },
				new() { IsSystem = true, LanguageId = 2, Text = "Kontakt", TranslationCode = "contact_email" },

				new() { IsSystem = true, LanguageId = 0, Text = "Pořadatel", TranslationCode = "contact_event_manager" },
				new() { IsSystem = true, LanguageId = 1, Text = "Organizer", TranslationCode = "contact_event_manager" },
				new() { IsSystem = true, LanguageId = 2, Text = "Veranstalter", TranslationCode = "contact_event_manager" },

				new() { IsSystem = true, LanguageId = 0, Text = "Tato akce je povolena pouze pro {0}", TranslationCode = "supported_lang_only" },
				new() { IsSystem = true, LanguageId = 1, Text = "This action is allowed for {0} only", TranslationCode = "supported_lang_only" },
				new() { IsSystem = true, LanguageId = 2, Text = "Diese Aktion ist nur für {0} zulässig", TranslationCode = "supported_lang_only" },

				new() { IsSystem = true, LanguageId = 0, Text = "Podpořte náš festival", TranslationCode = "donations_header" },
				new() { IsSystem = true, LanguageId = 1, Text = "Support our festival", TranslationCode = "donations_header" },
				new() { IsSystem = true, LanguageId = 2, Text = "Unterstützen Sie unser Festival", TranslationCode = "donations_header" },

				new() { IsSystem = true, LanguageId = 0, Text = "Peníze můžete zaslat pomocí přiložených QR kódů", TranslationCode = "donations_text" },
				new() { IsSystem = true, LanguageId = 1, Text = "You can send money using the attached QR codes", TranslationCode = "donations_text" },
				new() { IsSystem = true, LanguageId = 2, Text = "Mit den beigefügten QR-Codes können Sie Geld versenden", TranslationCode = "donations_text" },

				new() { IsSystem = true, LanguageId = 0, Text = "Na tomto zatím pracujeme", TranslationCode = "work_in_progress" },
				new() { IsSystem = true, LanguageId = 1, Text = "We are still working on this", TranslationCode = "work_in_progress" },
				new() { IsSystem = true, LanguageId = 2, Text = "Daran arbeiten wir noch", TranslationCode = "work_in_progress" },

				new() { IsSystem = true, LanguageId = 0, Text = "Odhlášení proběhlo úspěšně", TranslationCode = "logout_succes" },
				new() { IsSystem = true, LanguageId = 1, Text = "Logout was successful", TranslationCode = "logout_succes" },
				new() { IsSystem = true, LanguageId = 2, Text = "Die Abmeldung war erfolgreich", TranslationCode = "logout_succes" },

				new() { IsSystem = true, LanguageId = 0, Text = "Neznámá chyba při odhlášení", TranslationCode = "logout_fail" },
				new() { IsSystem = true, LanguageId = 1, Text = "Unknown error while logging out", TranslationCode = "logout_fail" },
				new() { IsSystem = true, LanguageId = 2, Text = "Unbekannter Fehler beim Abmelden", TranslationCode = "logout_fail" },
			};

			sysTrans.ForEach(tran =>
			{
				var actual = client.Search<Translation>(x => x
					.Query(q => q
						.Bool(bq => bq
							.Filter(
								fq => fq.Terms(t => t.Field(f => f.TranslationCode).Terms(tran.TranslationCode)),
								fq => fq.Terms(t => t.Field(f => f.LanguageId).Terms(tran.LanguageId))
							)
						)
					)
					.Size(1)).Documents.FirstOrDefault();

				if (actual is null)
				{
					var res = client.Index(tran, i => i);

					if (res.ServerError is not null)
					{
						throw new ElasticsearchClientException(JsonConvert.SerializeObject(res.ServerError));
					}
				}
			});


			return client;
		}
	}
}
