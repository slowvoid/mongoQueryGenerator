namespace QueryBuilder.ER
{
    public class RelationshipEnd
    {
        public Entity TargetEntity { get; set; }

        public RelationshipCardinality Cardinality { get; set; }
    }
}
