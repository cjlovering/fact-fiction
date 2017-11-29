import React from 'react';
import expect from 'expect';
import { mount } from 'enzyme';
import Button from '../../components/Button.jsx';

describe('Button', () => {

    let button;
    let content;
    let handleClick;    

    beforeEach(() => {
        handleClick = jest.fn();
        content = "text";
        button = mount(
            <Button handleClick={handleClick} content={content} />);
    });
    
    it('Button has a button', () => {
        expect(button.find('button').text()).toEqual(content);
    });

    it('Button requires handleClick prop', () => {
        expect(button.props().handleClick).toBeDefined();
    });

    it('Button requires content prop', () => {
        expect(button.props().content).toBeDefined();
    });

    it('Button click calls handleClick', () => {
        const _button = button.find('button').first();
        _button.simulate('click');
        expect(handleClick).toBeCalled();        
    });
});