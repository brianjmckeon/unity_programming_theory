
public class Orchard : Producer
{
    protected override int productionRate => 3;

    protected override int productionLimit => 3;

    protected override int dormantPeriod => 5;
}
