import * as yup from 'yup';

export const GroupOfSpecialitiesValidationSchema = yup.object().shape({
    id: yup.string().uuid(),
    name: yup.string().required('Введите название'),
    startDate: yup.date().required('Неверная дата'),
    enrollmentDate: yup.date().required('Неверная дата'),
    description: yup.string().nullable(true),
    isCompleted: yup.boolean().default(false),
    formOfEducation: yup.object().shape({
        id: yup.string().uuid(),
        year: yup.number(),
        isDailyForm: yup.boolean().required(),
        isBudget: yup.boolean().required(),
        isFullTime: yup.boolean().required()
    }).required(),
    specialities: yup.array().of(
        yup.object().shape({
            fullName: yup.string().required(),
            id: yup.string().uuid(),
            isSelected: yup.boolean().default(false)
        })
    ).test("at-least-one-true", 'Выберите хотя бы одну специальность', (obj) => {
        return Object.values(obj).some((value) => value.isSelected);
    }).required(),
    subjects: yup.array().of(
        yup.object().shape({
            name: yup.string().required(),
            id: yup.string().uuid(),
            isSelected: yup.boolean().default(false)
        })
    ).test("at-least-one-true", 'Выберите хотя бы один предмет', (obj) => {
        return Object.values(obj).some((value) => value.isSelected);
    }).required(),
});