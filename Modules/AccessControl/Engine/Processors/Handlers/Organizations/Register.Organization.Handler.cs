﻿using System.Collections.Generic;
using Trackwane.AccessControl.Domain.Organizations;
using Trackwane.AccessControl.Engine.Commands.Organizations;
using Trackwane.AccessControl.Engine.Services;
using Trackwane.Framework.Common;
using Trackwane.Framework.Common.Exceptions;
using Trackwane.Framework.Infrastructure.Requests;
using Trackwane.Framework.Interfaces;
using log4net;

namespace Trackwane.AccessControl.Engine.Processors.Handlers.Organizations
{
    public class RegisterOrganizationHandler : TransactionalHandler<RegisterOrganization>
    {
        private readonly IOrganizationService organizationService;
        /* Public */

        public RegisterOrganizationHandler(
            IProvideTransactions transaction,
            ILog log,
            IOrganizationService organizationService) : 
            base(transaction, log)
        {
            this.organizationService = organizationService;
        }

        protected override IEnumerable<DomainEvent> Handle(RegisterOrganization cmd, IRepository repository)
        {
            if (organizationService.IsExistingOrganization(cmd.ApplicationKey, cmd.OrganizationKey, repository))
            {
                throw new BusinessRuleException(PhraseBook.Generate(Message.DUPLICATE_ORGANIZATION_ID, cmd.OrganizationKey));
            }

            if (organizationService.IsExistingOrganizationName(cmd.ApplicationKey, cmd.Name, repository))
            {
                throw new BusinessRuleException(PhraseBook.Generate(Message.DUPLICATE_ORGANIZATION_NAME, cmd.Name));
            }

            var organization = new Organization(cmd.ApplicationKey, cmd.OrganizationKey, cmd.Name);

            repository.Persist(organization);

            return organization.GetUncommittedChanges();
        }
    }
}
