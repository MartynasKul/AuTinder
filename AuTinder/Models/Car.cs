namespace AuTinder.Models
{
    public enum BodyType
    {
        Sedan = 1,
        Hatchback = 2,
        Universal = 3,
        Single = 4,
        SUVCrossover = 5,
        Coupe = 6,
        Commercial = 7,
        Convertible = 8,
        Limousine = 9,
        Pickup = 10,
        PassengerMinibus = 11,
        CargoMinibus = 12,
        Other = 13,
    }
    public enum FuelType
    {
        Diesel = 1,
        DieselElectric = 2,
        Petrol = 3,
        PetrolGas = 4,
        PetrolElectric = 5,
        PetrolElectricGas = 6,
        Electric = 7,
        BioethanolE85 = 8,
        Hydrogen = 9,
        Other = 10
    }
    public enum DriveWheels
    {
        Front = 1,
        Rear = 2,
        All = 3
    }
    public enum Gearbox
    {
        Manual = 1,
        Automatic = 2
    }
    public enum SteeringWheelLocation
    {
        Left = 1,
        Right = 2
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

        public int UserId { get; set; }

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

        public Car(int id, string make, string model, BodyType bodyType, DateTime year, FuelType fuelType,
            int mileage, string color, DateTime inspection, DriveWheels driveWheels, Gearbox gearbox,
            int power, SteeringWheelLocation steeringWheelLocation, string outsideState, string extraFunc, float rating, int userid)
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
            UserId = userid;
        }
        #endregion

        public int CompareCars(Car other)
        {
            if (other == null)
                return 0;

            int matchCount = 0;

            if (this.Make == other.Make) matchCount++;
            if (this.Model == other.Model) matchCount++;
            if (this.BodyType == other.BodyType) matchCount++;
            if (Math.Abs(this.Year.Year - other.Year.Year) <= 2) matchCount++;
            if (this.FuelType == other.FuelType) matchCount++;
            if (this.Mileage - other.Mileage <= 10000) matchCount++;
            if (this.Color == other.Color) matchCount++;
            if (Math.Abs(this.Inspection.Year * 12 + this.Inspection.Month - other.Inspection.Year * 12 + other.Inspection.Month) <= 3) matchCount++;
            if (this.DriveWheels == other.DriveWheels) matchCount++;
            if (this.Gearbox == other.Gearbox) matchCount++;
            if (Math.Abs(this.Power - other.Power) <= 10) matchCount++;
            if (this.SteeringWheelLocation == other.SteeringWheelLocation) matchCount++;
            if (this.OutsideState == other.OutsideState) matchCount++;
            if (this.ExtraFunc == other.ExtraFunc) matchCount++;
            if (Math.Abs(this.Rating - other.Rating) <= 2) matchCount++;

            return matchCount;
        }

        public List<int> CompareRpeatsInCars(Car other, List<int> repeats)
        {
            if (other == null)
                return repeats;


            if (this.Make == other.Make) repeats[0]++;
            if (this.Model == other.Model) repeats[1]++;
            if (this.BodyType == other.BodyType) repeats[2]++;
            if (Math.Abs(this.Year.Year - other.Year.Year) <= 2) repeats[3]++;
            if (this.FuelType == other.FuelType) repeats[4]++;
            if (this.Mileage - other.Mileage <= 10000) repeats[5]++;
            if (this.Color == other.Color) repeats[6]++;
            if (Math.Abs(this.Inspection.Year * 12 + this.Inspection.Month - other.Inspection.Year * 12 + other.Inspection.Month) <= 3) repeats[7]++;
            if (this.DriveWheels == other.DriveWheels) repeats[8]++;
            if (this.Gearbox == other.Gearbox) repeats[9]++;
            if (Math.Abs(this.Power - other.Power) <= 10) repeats[10]++;
            if (this.SteeringWheelLocation == other.SteeringWheelLocation) repeats[11]++;
            if (this.OutsideState == other.OutsideState) repeats[12]++;
            if (this.ExtraFunc == other.ExtraFunc) repeats[13]++;
            if (Math.Abs(this.Rating - other.Rating) <= 2) repeats[14]++;

            return repeats;
        }

    }
}
