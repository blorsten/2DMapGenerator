using NUnit.Framework;
using MapGeneration.ChunkSystem;
using MapGeneration.Algorithm;

public class ChunkOpeningsTest
{
    [Test]
    public void Top_Connetion_Test()
    {
        ChunkOpenings opening = new ChunkOpenings();

        opening.SetConnection(PathAlgorithm.CardinalDirections.Top, ChunkOpenings.ConnectionType.Default, true);

        Assert.IsTrue(opening.TopConnection);
    }

    [Test]
    public void Bottom_Connetion_Test()
    {
        ChunkOpenings opening = new ChunkOpenings();

        opening.SetConnection(PathAlgorithm.CardinalDirections.Bottom, ChunkOpenings.ConnectionType.Default, true);

        Assert.IsTrue(opening.BottomConnetion);
    }

    [Test]
    public void Left_Connetion_Test()
    {
        ChunkOpenings opening = new ChunkOpenings();

        opening.SetConnection(PathAlgorithm.CardinalDirections.Left, ChunkOpenings.ConnectionType.Default, true);

        Assert.IsTrue(opening.LeftConnection);
    }

    [Test]
    public void Right_Connetion_Test()
    {
        ChunkOpenings opening = new ChunkOpenings();

        opening.SetConnection(PathAlgorithm.CardinalDirections.Right, ChunkOpenings.ConnectionType.Default, true);

        Assert.IsTrue(opening.RightConnection);
    }

    [Test]
    public void Top_Open_Test()
    {
        ChunkOpenings opening = new ChunkOpenings();

        opening.TopOpen = true;

        Assert.IsTrue(opening.TopOpen);
    }

    [Test]
    public void Bottom_Open_Test()
    {
        ChunkOpenings opening = new ChunkOpenings();

        opening.BottomOpen = true;

        Assert.IsTrue(opening.BottomOpen);
    }

    [Test]
    public void Left_Open_Test()
    {
        ChunkOpenings opening = new ChunkOpenings();

        opening.LeftOpen = true;

        Assert.IsTrue(opening.LeftOpen);
    }

    [Test]
    public void Right_Open_Test()
    {
        ChunkOpenings opening = new ChunkOpenings();

        opening.RightOpen = true;

        Assert.IsTrue(opening.RightOpen);
    }

    [Test]
    public void Is_Matching_Test()
    {
        ChunkOpenings opening1 = new ChunkOpenings();
        ChunkOpenings opening2 = new ChunkOpenings();

        opening2.TopOpen = true;
        opening2.LeftOpen = true;
        opening1.TopOpen = true;

        Assert.IsTrue(opening1.IsMatching(opening2));

    }

    [Test]
    public void Is_Open_Test()
    {
        ChunkOpenings opening = new ChunkOpenings();

        opening.TopOpen = true;
        Assert.IsTrue(opening.IsOpen(PathAlgorithm.CardinalDirections.Top));
        opening.BottomOpen = true;
        Assert.IsTrue(opening.IsOpen(PathAlgorithm.CardinalDirections.Bottom));
        opening.LeftOpen = true;
        Assert.IsTrue(opening.IsOpen(PathAlgorithm.CardinalDirections.Left));
        opening.RightOpen = true;
        Assert.IsTrue(opening.IsOpen(PathAlgorithm.CardinalDirections.Right));
    }

    [Test]
    public void Is_Dead_End_Test()
    {
        ChunkOpenings opening = new ChunkOpenings();
        opening.TopOpen = true;
        Assert.IsTrue(opening.IsDeadEnd());


        opening = new ChunkOpenings();
        opening.BottomOpen = true;
        Assert.IsTrue(opening.IsDeadEnd());


        opening = new ChunkOpenings();
        opening.LeftOpen = true;
        Assert.IsTrue(opening.IsDeadEnd());


        opening = new ChunkOpenings();
        opening.RightOpen = true;
        Assert.IsTrue(opening.IsDeadEnd());
    }
}
