﻿using System.Web.Http;
using Trackwane.Framework.Common;
using Trackwane.Framework.Infrastructure.Web.Security;
using Trackwane.Framework.Interfaces;
using Trackwane.Management.Contracts.Models;
using Trackwane.Management.Engine.Commands.Vehicles;
using Trackwane.Management.Engine.Queries.Vehicles;

namespace Trackwane.Management.Engine.Controllers
{
    [RoutePrefix("organizations/{organizationKey}")]
    public class VehiclesController : BaseManagementController
    {
        private const string RESOURCE_URL = "vehicles/{key}";
        private const string COLLECTION_URL = "vehicles";

        public VehiclesController(IExecutionEngine dispatcher) : base(dispatcher)
        {
        }

        [Secured, Managers, HttpPost, Route(RESOURCE_URL)]
        public void UpdateVehicle(string organizationKey, string key, UpdateVehicleModel model)
        {
            dispatcher.Handle(new UpdateVehicle(AppKeyFromHeader, CurrentClaims.UserId, organizationKey, key, model.Identifier));
        }

        [Secured, Viewers, HttpGet, Route(RESOURCE_URL)]
        public VehicleDetails FindById(string organizationKey, string key)
        {
            return dispatcher.Query<FindById>(AppKeyFromHeader, organizationKey).Execute(key);
        }

        [Secured, Viewers, HttpGet, Route(COLLECTION_URL)]
        public ResponsePage<VehicleSummary> FindBySearchCriteria(string organizationKey, SearchVehiclesModel model)
        {
            return
                dispatcher.Query<FindBySearchCriteria>(AppKeyFromHeader, organizationKey).Execute(new SearchVehiclesModel {Identifier = model.Identifier});
        }

        [Secured, Managers, HttpDelete, Route(RESOURCE_URL)]
        public void ArchiveBoundary(string organizationKey, string key)
        {
            dispatcher.Handle(new ArchiveVehicle(AppKeyFromHeader, CurrentClaims.UserId, organizationKey, key));
        }

        [Secured, Managers, HttpPost, Route(COLLECTION_URL)]
        public void RegisterVehicle(string organizationKey, RegisterVehicleModel model)
        {
            dispatcher.Handle(new RegisterVehicle(AppKeyFromHeader, CurrentClaims.UserId, organizationKey, model.Identifier, model.Key));
        }
    }
}
