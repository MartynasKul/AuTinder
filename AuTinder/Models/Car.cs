namespace AuTinder.Models
{
    public enum BodyType
    {
        Sedan,
        Hatchback,
        Universal,
        Single,
        SUVCrossover,
        Coupe,
        Commercial,
        Convertible,
        Limousine,
        Pickup,
        PassengerMinibus,
        CargoMinibus,
        Other
    }
    public enum FuelType
    {
        Diesel,
        DieselElectric,
        Petrol,
        PetrolGas,
        PetrolElectric,
        PetrolElectricGas,
        Electric,
        BioethanolE85,
        Hydrogen,
        Other
    }
    public enum DriveWheels
    {
        Front,
        Rear,
        All
    }
    public enum Gearbox
    {
        Manual,
        Automatic
    }
    public enum SteeringWheelLocation
    {
        Left,
        Right
    }


    public class Car
    {
        public int Id { get; set; }
        public string Make {  get; set; }
        public string Model {  get; set; }
        public BodyType BodyType { get; set; }
        public DateTime Year { get; set; } 
        public FuelType FuelType {  get; set; }
        public int Mileage {  get; set; }
        public string Color {  get; set; }
        public DateTime Inspection {  get; set; } // Technine apziura
        public DriveWheels DriveWheels {  get; set; }
        public Gearbox Gearbox { get; set; }
        public int Power {  get; set; }
        public SteeringWheelLocation SteeringWheelLocation {  get; set; }
        public string OutsideState {  get; set; }
        public string ExtraFunc {  get; set; }
        public float Rating {  get; set; }

        #region Constructors
        public Car() { }

        public Car(int id, string make, string model, BodyType bodyType, DateTime year, FuelType fuelType, 
            int mileage, string color, DateTime inspection, DriveWheels driveWheels, Gearbox gearbox,
            int power, SteeringWheelLocation steeringWheelLocation, string outsideState, string extraFunc, float rating )
        {
            Id = id;
            Make = make;
            Model = model;
            BodyType = bodyType;
            Year = year;
            FuelType = fuelType;
            Mileage = mileage;
            Color = color;
            Inspection = inspection;
            DriveWheels = driveWheels;
            Gearbox = gearbox;
            Power = power;
            SteeringWheelLocation = steeringWheelLocation;
            OutsideState = outsideState;
            ExtraFunc = extraFunc;
            Rating = rating;
        }
        #endregion
    }
}
