﻿using System.Linq;
using Marten;
using Trackwane.Framework.Common;
using Trackwane.Framework.Common.Interfaces;
using Trackwane.Framework.Infrastructure.Queries;
using Trackwane.Management.Contracts.Models;
using Trackwane.Management.Domain;

namespace Trackwane.Management.Engine.Queries.Locations
{
    public class FindBySearchCriteria : Query<ResponsePage<LocationSummary>>, IOrganizationQuery
    {
        public ResponsePage<LocationSummary> Execute()
        {
            return Execute(repository =>
            {
                var locations = repository.Query<Location>()
                  .Where(x => x.OrganizationKey == OrganizationKey)
                  .ToList();

                return new ResponsePage<LocationSummary>
                {
                    Items = locations.Select(x => new LocationSummary {Key = x.Key}).ToList(),
                    Total = locations.Count
                };
            });
        }

        public FindBySearchCriteria(IDocumentStore documentStore) : base(documentStore)
        {
        }

        public string OrganizationKey { get; set; }
    }
}
