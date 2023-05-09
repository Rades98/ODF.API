using System;
using System.Linq;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using ODF.Data.Contracts.Entities;
using ODF.Data.Contracts.Interfaces;
using ODF.Data.Elastic.Repos.Articles;
using ODF.Data.Elastic.Repos.Lineups;
using ODF.Data.Elastic.Repos.Translations;

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
			var elasticConf = configuration.GetSection(nameof(ElasticsearchSettings)).Get<ElasticsearchSettings>()
				?? throw new ArgumentNullException(nameof(ElasticsearchSettings));

			var connectionPool = new StaticConnectionPool(elasticConf.Nodes.Select(node => new Uri(node)));

			var settings = new ConnectionSettings(connectionPool)
				.PrettyJson()
				.DefaultIndex(elasticConf.DefaultIndex)
				.DefaultMappingFor<Article>(m => m.IndexName(elasticConf.DefaultIndex))
				.DefaultMappingFor<Translation>(m => m.IndexName("translations"))
				.DefaultMappingFor<LineupItem>(m => m.IndexName("lineup-item"))
				.BasicAuthentication("elastic", elasticConf.Password);

			var client = new ElasticClient(settings);

			client.Indices.Create(elasticConf.DefaultIndex, i => i.Map<Article>(x => x.AutoMap()));
			client.Indices.Create("translations", i => i.Map<Translation>(x => x.AutoMap()));
			client.Indices.Create("lineup-item", i => i.Map<LineupItem>(x => x.AutoMap()));

			services.AddSingleton<IElasticClient>(client);

			services.AddTransient<IArticleRepo, ArticleRepo>();
			//services.Decorate<IArticleRepo, ArticleRepoCache>();

			services.AddTransient<ITranslationRepo, TranslationRepo>();
			//services.Decorate<ITranslationRepo, TranslationRepoCache>();

			services.AddTransient<ILineupRepo, LineupRepo>();
			//services.Decorate<ILineupRepo, LineupRepoCahce>();

			return services;
		}
	}
}
