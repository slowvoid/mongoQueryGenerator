db.Person.aggregate([
    {"$match": {
        $expr: {
            $or: [
                { $eq: ['$age', 26] },
                { $eq: ['$age', 27] }
            ]
        }
    }}
])