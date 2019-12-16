db.Person.aggregate([
    {"$match": {
        $expr: {
            $not: [
                { $in: ['$age', [26, 27, 28, 29]] }
            ]
        }
    }}
]).pretty()