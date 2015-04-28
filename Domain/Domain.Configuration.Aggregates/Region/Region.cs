using System;

namespace Demo.Domain.Configuration.Region
{
    public class Region : Aggregates.Aggregate<Guid>, IRegion
    {
        private Region()
        {
        }

        public void Create(String Code, String Name, Guid? ParentId)
        {
            Apply<Events.Created>(e =>
            {
                e.RegionId = Id;
                e.Code = Code;
                e.Name = Name;
                e.ParentId = ParentId;
            });
        }

        public void ChangeName(String Name)
        {
            Apply<Events.NameChanged>(e =>
            {
                e.RegionId = Id;
                e.Name = Name;
            });
        }

        public void ChangeDescription(String Description)
        {
            Apply<Events.DescriptionChanged>(e =>
            {
                e.RegionId = Id;
                e.Description = Description;
            });
        }

        public void Destroy()
        {
            Apply<Events.Destroyed>(e =>
            {
                e.RegionId = Id;
            });
        }
    }
}