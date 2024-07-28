import * as yup from 'yup';

export const AdmissionValidationSchema = yup.object().shape({
    id: yup.string().uuid(),
    dateOfApplication: yup.date().required('Неверная дата'),
    passportID: yup.string().nullable(true),
    passportSeries: yup.string().nullable(true),
    passportNumber: yup.number().nullable(true),
    isTargeted: yup.bool().default(false),
    isWithoutEntranceExams: yup.bool().default(false),
    isOutOfCompetition: yup.bool().default(false),
    email: yup.string().email().nullable(true),
    student: yup.object().shape({
        id: yup.string().uuid(),
        name: yup.string().required('Введите имя'),
        surname: yup.string().required('Введите фамилию'),
        patronymic: yup.string().required('Введите отчество'),
        gpa: yup.number().min(0).required('Введите баллы'),
    }).required(),
    studentScores: yup.array().of(
        yup.object().shape({
            subject: yup.object().shape({
                name: yup.string().required()
            }),
            subjectId: yup.string().uuid(),
            score: yup.number().min(0).required('Введите баллы')
        })
    ).required(),
    specialityPriorities: yup.array().of(
        yup.object().shape({
            id: yup.string().uuid(),
            specialityName: yup.string().required(),
            priority: yup.number().min(0).required()
        })
    ).test("at-least-one-true", 'Выберите хотя бы одну специальность', (obj) => {
        return Object.values(obj).some((value) => value.priority > 0);
    }).required(),
});