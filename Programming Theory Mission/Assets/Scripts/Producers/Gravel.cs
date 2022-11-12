
public class Gravel : Producer
{
    protected override int productionRate => 1;
    protected override int productionLimit => 20;
    protected override int dormantPeriod => 10;
}
