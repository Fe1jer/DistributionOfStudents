import * as yup from 'yup';

export const SpecialityValidationSchema = yup.object().shape({
    id: yup.string().matches(/^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/i),
    fullName: yup.string().required('Введите название'),
    shortName: yup.string().nullable(true),
    code: yup.string().required('Введите код'),
    shortCode: yup.string().nullable(true),
    directionName: yup.string().nullable(true),
    directionCode: yup.string().nullable(true),
    specializationName: yup.string().nullable(true),
    specializationCode: yup.string().nullable(true),
    description: yup.string().nullable(true)
});