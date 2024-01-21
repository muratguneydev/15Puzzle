// namespace FifteenPuzzle.Tests.AutoFixture;

// using FifteenPuzzle.Game;
// using FifteenPuzzle.Solvers.ReinforcementLearning;
// using global::AutoFixture.Kernel;

// public class ActionQValuesSpecimenBuilder : ISpecimenBuilder
// {
// 	private static readonly Random Random = new();
// 	private static readonly int MaxNumber = Board.SideLength * Board.SideLength - 1;

//     public object Create(object request, ISpecimenContext context)
//     {
//         if (request is Type type && type == typeof(ActionQValues))
//         {        
//             return new ActionQValues(new[] {
// 				new ActionQValue(new Move(context.))
// 			}
				
// 				//Random.Next(1, MaxNumber));
//         }

//         return new NoSpecimen();
//     }
// }
