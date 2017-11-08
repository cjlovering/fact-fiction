import React from 'react';
import expect from 'expect';
import { mount } from 'enzyme';
import MainPane from '../components/MainPane.jsx';

describe('MainPane', () => {

    let mainPane;
    let addFact;
    let state;
    
    beforeEach(() => {
        state = ["hi"]
        addFact = jest.fn();
        mainPane = mount(<MainPane addFact={addFact} facts={state} />);
    });
    
    it('MainPane requires addFact prop', () => {
        expect(mainPane.props().addFact).toBeDefined();
    });

    it('MainPane requires facts prop', () => {
        expect(mainPane.props().facts).toBeDefined();
    });

    it('App renders nested components', () => {
        expect(mainPane.find('ListView').length).toEqual(1);
        expect(mainPane.find('InputPane').length).toEqual(1);
    });
});