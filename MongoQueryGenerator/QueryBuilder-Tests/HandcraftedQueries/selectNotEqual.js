db.Person.aggregate([
    {"$match": {
        $expr: {
            $ne: ['$age', 27]
        }
    }}
]).pretty()