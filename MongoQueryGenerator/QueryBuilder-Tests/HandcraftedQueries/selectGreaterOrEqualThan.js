db.Person.aggregate([
    {"$match": {
        $expr: {
            $gte: ['$age', 27]
        }
    }}
]).pretty()