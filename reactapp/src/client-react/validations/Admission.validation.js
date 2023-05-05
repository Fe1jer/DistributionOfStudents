import * as yup from 'yup';

export const AdmissionValidationSchema = yup.object().shape({
    id: yup.number(),
    dateOfApplication: yup.date().required('Неверная дата'),
    passportID: yup.string().nullable(true),
    passportSeries: yup.string().nullable(true),
    passportNumber: yup.number().nullable(true),
    email: yup.string().email().nullable(true),
    student: yup.object().shape({
        id: yup.number(),
        name: yup.string().required('Введите имя'),
        surname: yup.string().required('Введите фамилию'),
        patronymic: yup.string().required('Введите отчество'),
        gps: yup.number().min(0).required('Введите баллы'),
    }).required(),
    studentScores: yup.array().of(
        yup.object().shape({
            subject: yup.object().shape({
                name: yup.string().required()
            }),
            score: yup.number().min(0).required('Введите баллы')
        })
    ).required(),
    specialitiesPriority: yup.array().of(
        yup.object().shape({
            planId: yup.number().min(0).required(),
            nameSpeciality: yup.string().required(),
            priority: yup.number().min(0).required()
        })
    ).test("at-least-one-true", 'Выберите хотя бы одну специальность', (obj) => {
        return Object.values(obj).some((value) => value.priority > 0);
    }).required(),
});