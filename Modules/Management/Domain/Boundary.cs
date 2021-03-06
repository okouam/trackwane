﻿using System;
using Geo.Geometries;
using Trackwane.Framework.Common;
using Trackwane.Framework.Common.Exceptions;
using Trackwane.Framework.Common.Interfaces;
using Trackwane.Management.Contracts.Events;

namespace Trackwane.Management.Domain
{
    public class Boundary : AggregateRoot, IHaveName, IResource
    {
        /* Public */

        public bool IsArchived { get; set; }

        public enum BoundaryType
        {
            Geofence = 1,
            ExclusionZone = 2
        }

        public Polygon Coordinates { get; set; }

        public string Name { get; set; }

        public string OrganizationKey { get; set; }

        public BoundaryType Type { get; set; }
        
        public bool Contains(Point point)
        {
            throw new NotImplementedException();
        }

        public bool HasBeenEntered(Point previousCoords, Point newCoords)
        {
            if (IsMovementCheckPossible(previousCoords, newCoords))
            {
                // ReSharper disable PossibleInvalidOperationException
                //return !Contains(previousCoords.Longitude.Value, previousCoords.Latitude.Value) && Contain(previousCoords.Longitude.Value, previousCoords.Latitude.Value);
                // ReSharper restore PossibleInvalidOperationException
            }

            return false;
        }
        
        public bool HasBeenExited(Point previousCoords, Point newCoords)
        {
            if (!IsMovementCheckPossible(previousCoords, newCoords))
            {
                return false;
            }

            return false;
            // ReSharper disable PossibleInvalidOperationException
            // return Contains(previousCoords.Longitude.Value, previousCoords.Latitude.Value) && !Contain(newCoords.Longitude.Value, newCoords.Latitude.Value);
            // ReSharper restore PossibleInvalidOperationException
        }

        protected override void Apply(DomainEvent evt)
        {
            When((dynamic)evt);
        }

        public void UpdateCoordinates(Polygon newCoordinates)
        {
            if (IsArchived)
            {
                throw new BusinessRuleException();
            }

            Causes(new BoundaryUpdated
            {
                OrganizationKey = OrganizationKey,
                BoundaryKey = Key,
                Previous = new BoundaryUpdated.State {Name = Name, Coordinates = Coordinates},
                Current = new BoundaryUpdated.State {Name = Name, Coordinates = newCoordinates},
            });
        }

        public void UpdateName(string newName)
        {
            if (IsArchived)
            {
                throw new BusinessRuleException();
            }

            Causes(new BoundaryUpdated
            {
                BoundaryKey = Key,
                OrganizationKey = OrganizationKey,
                Previous = new BoundaryUpdated.State { Name = Name, Coordinates = Coordinates },
                Current = new BoundaryUpdated.State { Name = newName, Coordinates = Coordinates },
            });
        }

        public void Archive()
        {
            if (!IsArchived)
            {
                Causes(new BoundaryArchived
                {
                    BoundaryKey = Key,
                    OrganizationKey = OrganizationKey
                });
            }
        }

        public Boundary()
        {
        }

        public Boundary(string applicationKey, string id, string OrganizationKey, string name, Polygon coordinates)
        {
            Causes(new BoundaryCreated
            {
                BoundaryKey = id,
                ApplicationKey = applicationKey,
                Name = name,
                Coordinates = coordinates,
                OrganizationKey = OrganizationKey
            });
        }

        /* Private */

        private void When(BoundaryArchived evt)
        {
            IsArchived = true;
        }

        private void When(BoundaryUpdated evt)
        {
            if (!string.IsNullOrWhiteSpace(evt.Current.Name))
            {
                Name = evt.Current.Name;
            }

            if (evt.Current.Coordinates != null)
            {
                Coordinates = evt.Current.Coordinates;
            }
        }

        private void When(BoundaryCreated evt)
        {
            Key = evt.BoundaryKey;
            OrganizationKey = evt.OrganizationKey;
            ApplicationKey = evt.ApplicationKey;
            Coordinates = evt.Coordinates;
            Name = evt.Name;
        }

        private static bool IsMovementCheckPossible(Point previousCoords, Point newCoords)
        {
            return previousCoords != null && newCoords != null;
        }
    }
}
