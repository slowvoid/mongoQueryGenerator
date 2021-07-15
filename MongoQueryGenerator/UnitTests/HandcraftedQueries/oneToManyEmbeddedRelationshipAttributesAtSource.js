db.DocTypePerson.aggregate([
    { "$unwind": "$cars_multivalued_" },
    {
        "$lookup": {
            from: "DocTypeCar",
            localField: "cars_multivalued_._id",
            foreignField: "_id",
            as: "complete_car_data"
        }
    },
    { "$unwind": "$complete_car_data" },
    {
        "$group": {
            _id: "$_id",
            name: { $first: "$name" },
            data_Drives: {
                $push: {
                    "Car_carId": "$complete_car_data._id",
                    "Car_plate": "$complete_car_data.plate",
                    "Car_color": "$complete_car_data.color",
                    "Drives_Note": "$cars_multivalued_.note"
                }
            }
        }
    }
])