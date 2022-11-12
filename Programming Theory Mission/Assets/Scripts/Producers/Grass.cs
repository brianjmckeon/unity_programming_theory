
// Grass produces carrots.
public class Grass : Producer
{
    protected override int productionRate => 1;

    protected override int productionLimit => 5;

    protected override int dormantPeriod => 8;
}
