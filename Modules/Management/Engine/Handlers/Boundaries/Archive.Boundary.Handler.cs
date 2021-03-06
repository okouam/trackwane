﻿using System.Collections.Generic;
using log4net;
using Trackwane.Framework.Common;
using Trackwane.Framework.Common.Exceptions;
using Trackwane.Framework.Infrastructure.Requests;
using Trackwane.Framework.Interfaces;
using Trackwane.Management.Domain;
using Trackwane.Management.Engine.Commands.Boundaries;
using Trackwane.Management.Engine.Services;

namespace Trackwane.Management.Engine.Handlers.Boundaries
{
    public class ArchiveBoundaryHandler : Handler<ArchiveBoundary>
    {
        /* Public */

        public ArchiveBoundaryHandler(
            IProvideTransactions transaction,
            IExecutionEngine publisher,
            ILog log) :
            base(publisher, transaction, log)
        {
        }
        
        /* Protected */

        protected override IEnumerable<DomainEvent> Handle(ArchiveBoundary cmd, IRepository repository)
        {
            var boundary = repository.Find<Boundary>(cmd.BoundaryId, cmd.ApplicationKey);

            if (boundary == null)
            {
                throw new BusinessRuleException(PhraseBook.Generate(Message.UNKNOWN_BOUNDARY, cmd.BoundaryId));
            }

            boundary.Archive();

            repository.Persist(boundary);

            return boundary.GetUncommittedChanges();
        }
    }
}
