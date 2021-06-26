db.DocTypePerson.aggregate([
    {
        "$addFields": {
            data_Drives: {
                $map: {
                    input: "$cars_multivalued_",
                    as: "car",
                    in: {
                        Car_id: "$$car._id",
                        Car_color: "$$car.color",
                        Car_plate: "$$car.plate",
                        Drives_note: "$$car.note"
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