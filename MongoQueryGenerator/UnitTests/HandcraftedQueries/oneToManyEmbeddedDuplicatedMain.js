db.DocTypePerson.aggregate([
    {
        "$lookup": {
            from: "DocTypeCar",
            localField: "cars_multivalued_._id",
            foreignField: "_id",
            as: "data_Car"
        }
    },
    {
        "$addFields": {
            data_Drives: {
                $map: {
                    input: "$data_Car",
                    as: "car",
                    in: {
                        Drives_note: "$$car.note",
                        Car_id: "$$car._id",
                        Car_plate: "$$car.plate",
                        Car_color: "$$car.color"
                    }
                }
            }
        }
    },
    {
        "$project": {
            name: 1,
            "data_Drives.Car_plate": 1
        }
    }
])