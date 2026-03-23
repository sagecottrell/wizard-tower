namespace wizardtower;

public interface ICopy<TSelf> where TSelf : ICopy<TSelf>
{
    TSelf Copy();
}
