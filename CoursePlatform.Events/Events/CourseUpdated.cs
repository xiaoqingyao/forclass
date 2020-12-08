using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Events.Events
{
    public class CourseUpdated : ICoursePlatformEvent
    {
        public CourseUpdated(string catalogId, string catalogName, string name, int signatureId, string signatureName, string coverUrl, string intro, string goal, string id)
        {
            CatalogId = catalogId;
            CatalogName = catalogName;
            Name = name;
            SignatureId = signatureId;
            SignatureName = signatureName;
            CoverUrl = coverUrl;
            Intro = intro;
            Goal = goal;
            Id = id;
        }

        public string CatalogId { get; }
        public string CatalogName { get; }
        public string Name { get; }
        public int SignatureId { get; }
        public string SignatureName { get; }
        public string CoverUrl { get; }
        public string Intro { get; }
        public string Goal { get; }
        public string Id { get; }
    }
}
